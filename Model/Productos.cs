using BackendCancha.Model;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BackendCancha.Model 
{
    public class Productos
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres.")]
        public string Nombre { get; set; } = default!;

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [StringLength(500, ErrorMessage = "La descripción no puede exceder los 500 caracteres.")]
        public string Descripcion { get; set; } = default!;

        [Required(ErrorMessage = "El precio es obligatorio.")]
        [Range(0.01, 99999.99, ErrorMessage = "El precio debe ser un valor positivo.")]
        [Column(TypeName = "decimal(18, 2)")] // MEJORA: Asegura la precisión correcta para valores monetarios.
        public decimal Precio { get; set; } = default!;

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(0, int.MaxValue, ErrorMessage = "La cantidad no puede ser negativa.")]
        public int Cantidad { get; set; } = default!;

        [Url(ErrorMessage = "El formato de la URL de la imagen no es válido.")]
        public string? ImagenUrl { get; set; } = default!; // MEJORA: Se hace opcional (nullable).

        // --- Relación con Categoría ---
        [Required(ErrorMessage = "La categoría es obligatoria.")]
        public int CategoriaPId { get; set; } = default!;

        [ForeignKey("CategoriaPId")]
        public CategoriaP? CategoriaP { get; set; }
    }
}
