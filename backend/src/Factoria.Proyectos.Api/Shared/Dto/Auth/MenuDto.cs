namespace Factoria.Proyectos.Api.Shared.Dto.Auth
{
    public class MenuDto
    {
        public string CodigoModulo { get; set; } = null!;
        public string NombreModulo { get; set; } = null!;
        public string? RutaFrontend { get; set; }
    }
}