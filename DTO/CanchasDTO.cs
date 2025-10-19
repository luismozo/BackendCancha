using System.ComponentModel.DataAnnotations;

namespace BackendCancha.DTO
{
    public class CanchasDTO
    {
        [Required] public string Nombre { get; set; } = default!;
        [Required] public string Descripcion { get; set; } = default!;
        public string? ImagenUrl { get; set; }
        [Required] public string Estado { get; set; } = default!;
        [Required] public decimal AlquilerporHoras { get; set; }
        public DateTime? Fechaasintencia { get; set; }
    }
}
