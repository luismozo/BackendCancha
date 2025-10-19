using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCancha.Model
{
    public class ReservaDetalle
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int ReservaId { get; set; }

        [ForeignKey("ReservaId")]
        public Reserva? Reserva { get; set; }

        // Puede ser un producto o una cancha
        public int? ProductoId { get; set; }
        [ForeignKey("ProductoId")]
        public Productos? Producto { get; set; }

        public int? CanchaId { get; set; }
        [ForeignKey("CanchaId")]
        public Canchas? Cancha { get; set; }

        [Required]
        public int Cantidad { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal PrecioUnitario { get; set; }
    }
}
