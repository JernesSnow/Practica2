using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccesoADatos.Models
{
    [Table("Estudiantes")]
    public class Estudiante
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150, MinimumLength = 3)]
        public string NombreCompleto { get; set; } = string.Empty;

        [Required, StringLength(150)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required, StringLength(20)]
        public string Telefono { get; set; } = string.Empty;

        [Required, StringLength(80)]
        public string Carrera { get; set; } = string.Empty;

        [Required, StringLength(200)]
        public string PasswordHash { get; set; } = string.Empty;

        public int RolId { get; set; }
        public Rol Rol { get; set; } = null!;

        [Required, StringLength(20)]
        public string EstadoSolicitud { get; set; } = "Pendiente";

        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
