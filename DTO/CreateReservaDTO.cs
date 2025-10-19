using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace BackendCancha.DTO
{
    // DTO que representa un item individual dentro del carrito enviado desde el frontend.
    public class CarritoItemInputDTO
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Tipo { get; set; } = default!;

        [Required]
        [Range(1, int.MaxValue)]
        public int CantidadSeleccionada { get; set; }

        // Aceptamos estos datos adicionales que envía el frontend.
        public string Nombre { get; set; } = default!;
        public decimal Precio { get; set; }
    }

    // DTO principal que el controlador espera recibir.
    public class CreateReservaDTO
    {
        [Required]
        public string MetodoPago { get; set; } = default!;

        [Required]
        [MinLength(1, ErrorMessage = "El carrito no puede estar vacío.")]
        // CORRECCIÓN: Se cambia el nombre de la propiedad a 'items' (minúscula)
        // para que coincida con el payload JSON del frontend.
        public List<CarritoItemInputDTO> items { get; set; } = new List<CarritoItemInputDTO>();
    }
}

