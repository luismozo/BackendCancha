using System.ComponentModel.DataAnnotations;

namespace BackendCancha.DTO
{
    public class LoginDTO
    {
        [Required, EmailAddress]
        public string Correo { get; set; } = default!;
        [Required]
        public string Password { get; set; } = default!;
    }
}
