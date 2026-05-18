using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("usuario_perfil", Schema = "seguridad")]
    public class UsuarioPerfil
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [Column("perfil_id")]
        public int PerfilId { get; set; }
        public Perfil Perfil { get; set; } = null!;
    }
}