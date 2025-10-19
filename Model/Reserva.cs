using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BackendCancha.Model
{
    public class Reserva
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int UsuarioId { get; set; }

        [ForeignKey("UsuarioId")]
        public Usuario? Usuario { get; set; }

        [Required]
        public DateTime FechaCreacion { get; set; } = DateTime.UtcNow;

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Total { get; set; }

        [Required]
        public string MetodoPago { get; set; } = default!;

        // Relación con los detalles de la reserva
        // Inicializamos la colección para evitar advertencias de nulabilidad.
        public ICollection<ReservaDetalle> Detalles { get; set; } = new List<ReservaDetalle>();
    }
}