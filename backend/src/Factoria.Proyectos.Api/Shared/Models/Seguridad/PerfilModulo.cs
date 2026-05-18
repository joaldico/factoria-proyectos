using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("perfil_modulo", Schema = "seguridad")]
    public class PerfilModulo
    {
        [Column("perfil_id")]
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;

        [Column("modulo_id")]
        public int ModuloId { get; set; }
        public Modulo Modulo { get; set; } = null!;
    }
}