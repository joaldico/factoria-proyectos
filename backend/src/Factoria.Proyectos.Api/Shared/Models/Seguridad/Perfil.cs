using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("perfiles", Schema = "seguridad")]
    public class Perfil
    {
        [Key]
        [Column("perfil_id")]
        public int PerfilId { get; set; }

        [Column("codigo_perfil")]
        public string CodigoPerfil { get; set; } = string.Empty;

        [Column("nombre_perfil")]
        public string NombrePerfil { get; set; } = string.Empty;

        [Column("activo")]
        public bool Activo { get; set; }

        public ICollection<UsuarioPerfil> UsuarioPerfiles { get; set; } = new List<UsuarioPerfil>();
        public ICollection<PerfilModulo> PerfilModulos { get; set; } = new List<PerfilModulo>();
    }
}