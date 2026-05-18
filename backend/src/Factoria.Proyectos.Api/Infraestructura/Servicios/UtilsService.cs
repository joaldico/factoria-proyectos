using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Factoria.Proyectos.Api.Shared.Dto.Genericas;
using System.Security.Cryptography;
using System.Text;

namespace Factoria.Proyectos.Api.Infraestructura.Servicios
{
    public class UtilsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UtilsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int ObtenerUsuarioIdDelToken()
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user == null)
                throw new UnauthorizedAccessException("Usuario no autenticado en el contexto.");

            var claimId = user.FindFirstValue(ClaimTypes.NameIdentifier) ?? user.FindFirstValue("sub");
            
            if (int.TryParse(claimId, out int usuarioId))
                return usuarioId;
                
            throw new UnauthorizedAccessException("El token no contiene un ID de usuario válido.");
        }

        public string GenerarPasswordAleatorio(int longitud = 12)
        {
            const string caracteres = "ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz23456789!@#$%";
            var bytes = new byte[longitud];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);

            var sb = new StringBuilder(longitud);
            for (int i = 0; i < longitud; i++)
            {
                sb.Append(caracteres[bytes[i] % caracteres.Length]);
            }
            return sb.ToString();
        }

        public IActionResult ValidarModelo(ControllerBase controller)
        {
            if (!controller.ModelState.IsValid)
            {
                var mensaje = string.Join("; ", controller.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

                return controller.BadRequest(new RespuestaSimpleBE
                {
                    rpt = 106,
                    mensaje = mensaje,
                    data = null
                });
            }
            return null;
        }

        public string MensajeCrudRespuesta(bool esExito, int id)
        {
            if (!esExito)
                return "No se pudo guardar el registro.";

            return id == 0
                ? "Registro exitoso."
                : "Actualización exitosa.";
        }
    }
}