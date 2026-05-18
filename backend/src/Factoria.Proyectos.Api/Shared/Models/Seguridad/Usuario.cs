using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Factoria.Proyectos.Api.Shared.Models.Seguridad
{
    [Table("usuarios", Schema = "seguridad")]
    public class Usuario
    {
        [Key]
        [Column("usuario_id")]
        public int UsuarioId { get; set; }

        [Column("codigo_usuario")]
        public string CodigoUsuario { get; set; } = string.Empty;

        [Column("nombre_completo")]
        public string NombreCompleto { get; set; } = string.Empty;

        [Column("correo")]
        public string Correo { get; set; } = string.Empty;

        [Column("contrasena_hash")]
        public string ContrasenaHash { get; set; } = string.Empty;

        [Column("salt")]
        public string Salt { get; set; } = string.Empty;

        [Column("requiere_mfa")]
        public bool RequiereMfa { get; set; }

        [Column("estado_secret_mfa")]
        public string EstadoSecretMfa { get; set; } = "PEN";

        [Column("empresa_id")]
        public int? EmpresaId { get; set; }

        [Column("activo")]
        public bool Activo { get; set; }

        // Navegación EF Core
        public ICollection<UsuarioPerfil> UsuarioPerfiles { get; set; } = new List<UsuarioPerfil>();
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}