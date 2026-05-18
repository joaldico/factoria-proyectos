using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using Factoria.Proyectos.Api.Infraestructura.Data;
using Factoria.Proyectos.Api.Infraestructura.Servicios;

var builder = WebApplication.CreateBuilder(args);

var rutaEnv = builder.Configuration["RutaEnv"] ?? ".env";
if (File.Exists(rutaEnv))
{
    Console.WriteLine($"[Config] Cargando .env desde: {rutaEnv}");
    DotNetEnv.Env.Load(rutaEnv);
}

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHealthChecks();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Factoria API", Version = "v1" });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication", 
        Description = "Pega tu token JWT generado",
        In = ParameterLocation.Header, 
        Type = SecuritySchemeType.Http,
        Scheme = "bearer", 
        BearerFormat = "JWT"
    };
    c.AddSecurityDefinition("Bearer", securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement { 
        { 
            new OpenApiSecurityScheme { 
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } 
            }, 
            new string[] {} 
        } 
    });
});

var conexionBD = builder.Configuration["_ConexionBD"] 
                 ?? builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(conexionBD))
{
    throw new Exception("[CRITICAL] No se encontró la cadena de conexión '_ConexionBD' o 'DefaultConnection'.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(conexionBD));

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<PBKDF2>();
builder.Services.AddScoped<DapperRepository>();
builder.Services.AddScoped<ArchivoTexto>();
builder.Services.AddScoped<TokenService>();
builder.Services.AddScoped<ErrorService>();
builder.Services.AddScoped<AwsS3Helper>();
builder.Services.AddScoped<UtilsService>();

builder.Services.Configure<Log>(builder.Configuration.GetSection("LogSettings"));
builder.Services.Configure<Jwt>(builder.Configuration.GetSection("Jwt"));

var jwtSettings = builder.Configuration.GetSection("Jwt").Get<Jwt>();
var keyString = builder.Configuration["_JwtKey"];
if (string.IsNullOrEmpty(keyString)) throw new Exception("Falta _JwtKey en la configuración.");
var key = Encoding.UTF8.GetBytes(keyString);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true, 
            ValidateAudience = true, 
            ValidateLifetime = true, 
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings?.Issuer ?? "Factoria.Api", 
            ValidAudience = jwtSettings?.Audience ?? "Factoria.Client",
            IssuerSigningKey = new SymmetricSecurityKey(key), 
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AccesoSeguroFactoria", policy => 
    {
        policy.RequireAuthenticatedUser();
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("PermitirFrontend", policy =>
    {
        policy.AllowAnyOrigin() 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
    options.KnownNetworks.Clear(); 
    options.KnownProxies.Clear();
});

var app = builder.Build();

app.UseForwardedHeaders();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors("PermitirFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHealthChecks("/health");

app.Run();