using System.ComponentModel.DataAnnotations;

namespace BackendCancha.DTO
{
    // DTO para crear o actualizar una categoría.
    public class CategoriaPDTO
    {
        [Required, StringLength(50)]
        public string Nombre { get; set; } = default!;
    }
}
