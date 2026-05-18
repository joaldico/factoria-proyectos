using System.Runtime.Serialization;

namespace Factoria.Proyectos.Api.Shared.Dto.Genericas
{
    public class RespuestaBE<T>
    {
        public int? rpt { get; set; }
        public string? mensaje { get; set; }
        public T? data { get; set; }
        public string? detalleError { get; set; }
    }

    public class RespuestaSimpleBE
    {
        public int? rpt { get; set; }
        public string? mensaje { get; set; }
        public object? data { get; set; }
        public string? detalleError { get; set; }
    }
}
