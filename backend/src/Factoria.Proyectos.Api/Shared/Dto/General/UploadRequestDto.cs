namespace Factoria.Proyectos.Api.Shared.Dto.General;

public class UploadBase64RequestDto
{
    public string Base64 { get; set; } = string.Empty;
    public string TipoArchivo { get; set; } = string.Empty; // Ej: "image/png", "application/pdf"
    public string Folder { get; set; } = "general"; // Ej: "productos", "profesionales"
}

public class UploadMultiplesBase64RequestDto
{
    public List<UploadBase64RequestDto> Archivos { get; set; } = new();
}