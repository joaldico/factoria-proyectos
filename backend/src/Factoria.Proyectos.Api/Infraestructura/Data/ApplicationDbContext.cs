using Microsoft.EntityFrameworkCore;

namespace Factoria.Proyectos.Api.Infraestructura.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        { 
        }

        // TODO: Los DbSet definitivos (Usuarios, Roles, Tarifas, Trayectos) 
        // se autogenerarán aquí al ejecutar el comando de Scaffolding.

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Dado que Flyway maneja la creación y restricciones de la base de datos,
            // aquí mapearemos los esquemas específicos de PostgreSQL si es necesario
            // (por ejemplo: modelBuilder.HasDefaultSchema("public");)
        }
    }
}