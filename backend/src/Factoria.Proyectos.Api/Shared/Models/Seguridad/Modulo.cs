using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("modulos", Schema = "seguridad")]
    public class Modulo
    {
        [Key]
        [Column("modulo_id")]
        public int ModuloId { get; set; }

        [Column("codigo_modulo")]
        public string CodigoModulo { get; set; } = string.Empty;

        [Column("nombre_modulo")]
        public string NombreModulo { get; set; } = string.Empty;

        [Column("ruta_frontend")]
        public string RutaFrontend { get; set; } = string.Empty;

        [Column("activo")]
        public bool Activo { get; set; }

        public ICollection<PerfilModulo> PerfilModulos { get; set; } = new List<PerfilModulo>();
    }
}