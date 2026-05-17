namespace Factoria.Proyectos.Api.Shared.Dto.Genericas
{
    public class TokenAutenticarResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public string QrBase64 { get; set; } = string.Empty;
    }

    public class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
