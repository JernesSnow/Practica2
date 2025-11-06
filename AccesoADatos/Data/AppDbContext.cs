using System.Collections.Generic;
using System.Reflection.Emit;
using AccesoADatos.Models;
using Microsoft.EntityFrameworkCore;

namespace AccesoADatos.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<Estudiante> Estudiantes { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed de roles (coinciden con el SQL)
            modelBuilder.Entity<Rol>().HasData(
                new Rol { Id = 1, Nombre = "Participante" },
                new Rol { Id = 2, Nombre = "Administrador" }
            );

            modelBuilder.Entity<Estudiante>()
                .HasIndex(e => e.Correo)
                .IsUnique();
        }
    }
}
