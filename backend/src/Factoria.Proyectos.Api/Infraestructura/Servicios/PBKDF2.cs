using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class PBKDF2
    {
        private const int Iteraciones = 30000;
        private const int TamañoHash = 32;
        private const int TamañoSalt = 32;


        public string GenerarSalt()
        {
            byte[] saltBytes = RandomNumberGenerator.GetBytes(TamañoSalt);
            return Convert.ToHexString(saltBytes).ToLower();
        }


        public string GenerarKeySecret(string password, string saltHex)
        {
            byte[] saltBytes = Convert.FromHexString(saltHex);


            byte[] hashBytes = Rfc2898DeriveBytes.Pbkdf2(
            password,
            saltBytes,
            Iteraciones,
            HashAlgorithmName.SHA256,
            TamañoHash
            );


            return Convert.ToHexString(hashBytes).ToLower();
        }
    }
}