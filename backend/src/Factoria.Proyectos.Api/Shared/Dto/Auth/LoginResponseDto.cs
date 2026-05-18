namespace Factoria.Proyectos.Api.Shared.Dto.Auth
{
    public class LoginResponseDto
    {
        public int UsuarioId { get; set; }
        public string CodigoUsuario { get; set; } = null!;
        public string NombreCompleto { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int? EmpresaId { get; set; }
        
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        
        public bool RequiereMfa { get; set; }
        public string EstadoSecretMfa { get; set; } = null!;
        
        public List<string> Perfiles { get; set; } = new List<string>();
        public List<MenuDto> Modulos { get; set; } = new List<MenuDto>();
    }
}