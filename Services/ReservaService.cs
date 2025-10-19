using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Services
{
    public class ReservaService : IReservaService
    {
        private readonly AppDBContext _context;

        public ReservaService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<Reserva> CreateReservaAsync(CreateReservaDTO dto, int usuarioId)
        {
            // MEJORA: Se optimiza la obtención de datos para evitar múltiples llamadas a la BD.

            // 1. Recopilar todos los IDs necesarios antes del bucle.
            var productIds = dto.items.Where(i => i.Tipo.ToLower() == "producto").Select(i => i.Id).ToList();
            var canchaIds = dto.items.Where(i => i.Tipo.ToLower() == "cancha").Select(i => i.Id).ToList();

            // 2. Hacer solo dos consultas a la base de datos para obtener toda la información.
            // Se usa un diccionario para un acceso súper rápido por ID.
            var productosEnCarrito = await _context.Productos
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var canchasEnCarrito = await _context.Canchas
                .Where(c => canchaIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            var nuevaReserva = new Reserva
            {
                UsuarioId = usuarioId,
                MetodoPago = dto.MetodoPago,
                Total = 0
            };

            decimal totalCalculado = 0;

            foreach (var item in dto.items)
            {
                var detalle = new ReservaDetalle
                {
                    Reserva = nuevaReserva,
                    Cantidad = item.CantidadSeleccionada,
                };

                if (item.Tipo.ToLower() == "producto")
                {
                    // 3. Buscar el producto en memoria , no en la BD.
                    if (!productosEnCarrito.TryGetValue(item.Id, out var producto))
                    {
                        throw new Exception($"Producto con ID {item.Id} no encontrado.");
                    }
                    detalle.ProductoId = producto.Id;
                    detalle.PrecioUnitario = producto.Precio;
                }
                else if (item.Tipo.ToLower() == "cancha")
                {
                    // 4. Buscar la cancha en memoria.
                    if (!canchasEnCarrito.TryGetValue(item.Id, out var cancha))
                    {
                        throw new Exception($"Cancha con ID {item.Id} no encontrada.");
                    }
                    detalle.CanchaId = cancha.Id;
                    detalle.PrecioUnitario = cancha.AlquilerporHoras;
                }

                totalCalculado += detalle.PrecioUnitario * detalle.Cantidad;
                _context.ReservaDetalle.Add(detalle);
            }

            nuevaReserva.Total = totalCalculado;
            _context.Reserva.Add(nuevaReserva);

            // 5. Guardar todos los cambios en la base de datos una sola vez.
            await _context.SaveChangesAsync();
            return nuevaReserva;
        }
    }
}
