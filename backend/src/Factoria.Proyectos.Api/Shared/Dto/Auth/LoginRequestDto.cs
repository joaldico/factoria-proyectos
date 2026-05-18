using System.ComponentModel.DataAnnotations;

namespace Factoria.Proyectos.Api.Shared.Dto.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "El correo o código de usuario es obligatorio.")]
        public string Usuario { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string Password { get; set; } = null!;
    }
}