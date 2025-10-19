using System.ComponentModel.DataAnnotations;

namespace BackendCancha.Model
{
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(50)]
        public string Nombre { get; set; } = default!;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [StringLength(50)]
        public string Apellido { get; set; } = default!;

        [Required(ErrorMessage = "El correo es obligatorio.")]
        [EmailAddress]
        public string Correo { get; set; } = default!;

        [Phone]
        public string? Telefono { get; set; }

        [Required]
        public string PasswordHash { get; set; } = default!;

        public string Rol { get; set; } = "Cliente";
    }
}