using System.ComponentModel.DataAnnotations;

namespace BackendCancha.DTO
{
    public class ProductosDTO
    {
        [Required] public string Nombre { get; set; } = default!;
        [Required] public string Descripcion { get; set; } = default!;
        [Required] public decimal Precio { get; set; }
        [Required] public int Cantidad { get; set; }
        public string? ImagenUrl { get; set; }
        [Required] public int CategoriaPId { get; set; }
    }
}
