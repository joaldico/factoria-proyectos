namespace Factoria.Proyectos.Api.Shared.Dto.Genericas
{
    public class Jwt
    {
        public string Key { get; set; } = "";
        public string Issuer { get; set; } = "";
        public string Audience { get; set; } = "";
        public int ExpireMinutes { get; set; }
        public int RefreshExpireDays { get; set; }
    }
}
