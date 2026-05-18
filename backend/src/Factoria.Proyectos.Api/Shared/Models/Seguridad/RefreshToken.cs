using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("refresh_tokens", Schema = "seguridad")]
    public class RefreshToken
    {
        [Key]
        [Column("token")]
        public string Token { get; set; } = string.Empty;

        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        [Column("fecha_expiracion")]
        public DateTime FechaExpiracion { get; set; }

        [Column("origen")]
        public string Origen { get; set; } = string.Empty;

        [Column("indicador_mfa_validado")]
        public string IndicadorMfaValidado { get; set; } = string.Empty;
    }
}