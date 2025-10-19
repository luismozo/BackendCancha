using System.ComponentModel.DataAnnotations;

namespace BackendCancha.DTO
{
    public class UsuarioDTO
    {
        [Required]
        public string Nombre { get; set; } = default!;
        [Required]
        public string Apellido { get; set; } = default!;
        [Required, EmailAddress]
        public string Correo { get; set; } = default!;
        public string? Telefono { get; set; }
        [Required, MinLength(6)]
        public string PasswordHash { get; set; } = default!;
        [Required]
        public string Rol { get; set; } = default!;
    }
}
