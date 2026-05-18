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

        public string GenerarJwt(string usuarioId, string codigoUsuario, string nombreCompleto)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuarioId),
                new Claim(JwtRegisteredClaimNames.UniqueName, codigoUsuario),
                new Claim("NombreCompleto", nombreCompleto),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var keyString = _configuration["_JwtKey"];
            if (string.IsNullOrEmpty(keyString)) throw new Exception("La llave JWT no está configurada.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyString));
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