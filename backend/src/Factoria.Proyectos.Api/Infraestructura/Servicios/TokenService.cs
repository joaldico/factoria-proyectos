using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class TokenService
    {
        private readonly Jwt _settings;
        private readonly IConfiguration _configuration;

        public TokenService(IOptions<Jwt> options, IConfiguration configuration)
        {
            _settings = options.Value;
            _configuration = configuration;
        }


        public string GenerarJwt(string usuarioId, string usuario, string IndicadorMfaValidado,
            string IndicadorOlvidoContrasena = "0",
            string IndicadorContrasenaExpirada = "0",
            string IndicadorContrasenaTemporal = "0",
            string EmpresaId = "0", 
            string tenantAlias = "0")
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId),
                new Claim(JwtRegisteredClaimNames.UniqueName, usuario),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("IndicadorMfaValidado", IndicadorMfaValidado),
                new Claim("IndicadorOlvidoContrasena", IndicadorOlvidoContrasena),
                new Claim("IndicadorContrasenaExpirada", IndicadorContrasenaExpirada),
                new Claim("IndicadorContrasenaTemporal", IndicadorContrasenaTemporal),
                new Claim("EmpresaId", EmpresaId),         
                new Claim("TenantAlias", tenantAlias)     
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["_JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_settings.ExpireMinutes),
            signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


        public string GenerarRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
    }
}
