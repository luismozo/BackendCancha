using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.Json.Serialization; // <-- ¡ESTE 'using' ES VITAL!

namespace BackendCancha.DTO
{
    public class CarritoItemInputDTO
    {
        [Required]
        [JsonPropertyName("id")] // Mapea 'id' (JS) a 'Id' (C#)
        public int Id { get; set; }

        [Required]
        [JsonPropertyName("tipo")]
        public string Tipo { get; set; } = default!;

        [Required]
        [Range(1, int.MaxValue)]
        [JsonPropertyName("cantidadSeleccionada")]
        public int CantidadSeleccionada { get; set; }

        [Required] // Lo hacemos requerido
        [JsonPropertyName("nombre")]
        public string Nombre { get; set; } = default!;

        [Required]
        [JsonPropertyName("precio")]
        public decimal Precio { get; set; }

        [JsonPropertyName("fechaReserva")]
        public DateTime? FechaReserva { get; set; }

        [JsonPropertyName("horaInicio")]
        public string? HoraInicio { get; set; }

        [JsonPropertyName("horaFin")]
        public string? HoraFin { get; set; }
    }

    public class CreateReservaDTO
    {
        [Required]
        [JsonPropertyName("metodoPago")]
        public string MetodoPago { get; set; } = default!;

        [Required]
        [MinLength(1, ErrorMessage = "El carrito no puede estar vacío.")]
        [JsonPropertyName("items")]
        public List<CarritoItemInputDTO> items { get; set; } = new List<CarritoItemInputDTO>();
    }
}