using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCancha.Model 
{
    public class Canchas
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = default!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = default!;

        [Url(ErrorMessage = "El formato de la URL de la imagen no es válido.")]
        public string? ImagenUrl { get; set; } = default!; // MEJORA: Se hace opcional (nullable) por si una cancha no tiene imagen.

        [Required(ErrorMessage = "El estado es obligatorio.")]
        public string Estado { get; set; } = default!;

        [Required(ErrorMessage = "El precio de alquiler es obligatorio.")]
        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
        [Column(TypeName = "decimal(18, 2)")] // MEJORA: Asegura la precisión correcta en la base de datos para valores monetarios.
        public decimal AlquilerporHoras { get; set; } = default!;

        // MEJORA: Se hace opcional (nullable), ya que una cancha no siempre tiene una fecha de asistencia asociada.
        // Este campo es más relacionado a una reserva específica que a la cancha en sí.
        public DateTime? Fechaasintencia { get; set; } = default!;
    }
}
