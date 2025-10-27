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

        // --- TU MÉTODO OPTIMIZADO (ESTÁ PERFECTO) ---
        public async Task<Reserva> CreateReservaAsync(CreateReservaDTO dto, int usuarioId)
        {
            // 1. Recopilar todos los IDs necesarios antes del bucle.
            var productIds = dto.items.Where(i => i.Tipo.ToLower() == "producto").Select(i => i.Id).ToList();
            var canchaIds = dto.items.Where(i => i.Tipo.ToLower() == "cancha").Select(i => i.Id).ToList();

            // 2. Hacer solo dos consultas a la base de datos.
            var productosEnCarrito = await _context.Productos
                .Where(p => productIds.Contains(p.Id))
                .ToDictionaryAsync(p => p.Id);

            var canchasEnCarrito = await _context.Canchas
                .Where(c => canchaIds.Contains(c.Id))
                .ToDictionaryAsync(c => c.Id);

            // CÓDIGO CORREGIDO para ReservaService.cs

            var nuevaReserva = new Reserva
            {
                UsuarioId = usuarioId,
                MetodoPago = dto.MetodoPago,
                Total = 0
                // Los campos 'Estado' y 'FechaCreacion' se llenan solos
                // gracias a los valores por defecto en el modelo Reserva.cs
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
                    if (!productosEnCarrito.TryGetValue(item.Id, out var producto))
                    {
                        throw new Exception($"Producto con ID {item.Id} no encontrado.");
                    }
                    detalle.ProductoId = producto.Id;
                    detalle.PrecioUnitario = producto.Precio;
                }
                else if (item.Tipo.ToLower() == "cancha")
                {
                    if (!canchasEnCarrito.TryGetValue(item.Id, out var cancha))
                    {
                        throw new Exception($"Cancha con ID {item.Id} no encontrada.");
                    }

                    detalle.CanchaId = cancha.Id;
                    detalle.PrecioUnitario = cancha.AlquilerporHoras;

                    // --- CÓDIGO DEFENSIVO (LA SOLUCIÓN REAL) ---

                    // 1. Validar HoraInicio
                    if (string.IsNullOrEmpty(item.HoraInicio) || !TimeSpan.TryParse(item.HoraInicio, out TimeSpan horaInicioParsed))
                    {
                        // Si HoraInicio está vacío, nulo, o no es un formato de hora válido (ej: "14:00")
                        throw new Exception($"La hora de inicio '{item.HoraInicio ?? "NULL"}' para la cancha '{cancha.Nombre}' no es válida.");
                    }

                    // 2. Validar HoraFin
                    if (string.IsNullOrEmpty(item.HoraFin) || !TimeSpan.TryParse(item.HoraFin, out TimeSpan horaFinParsed))
                    {
                        // Si HoraFin está vacío, nulo, o no es un formato de hora válido
                        throw new Exception($"La hora de fin '{item.HoraFin ?? "NULL"}' para la cancha '{cancha.Nombre}' no es válida.");
                    }

                    // 3. Asignar los valores (ahora sabemos que son válidos)
                    detalle.FechaReservaCancha = item.FechaReserva;
                    detalle.HoraInicio = horaInicioParsed;
                    detalle.HoraFin = horaFinParsed;
                }

                totalCalculado += detalle.PrecioUnitario * detalle.Cantidad;
                _context.ReservaDetalle.Add(detalle);
            }

            nuevaReserva.Total = totalCalculado;
            _context.Reserva.Add(nuevaReserva);

            await _context.SaveChangesAsync();
            return nuevaReserva;
        }

        // --- IMPLEMENTACIONES AÑADIDAS ---

        // Método para OBTENER TODAS las reservas (para el Admin)
        public async Task<IEnumerable<Reserva>> GetAllReservasAsync()
        {
            // Incluimos al Usuario para poder mostrar su nombre en la tabla del admin
            // Incluimos Detalles y sus Canchas/Productos para poder mostrar qué se reservó
            return await _context.Reserva
                .Include(r => r.Usuario)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Cancha)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Producto)
                   .OrderByDescending(r => r.FechaCreacion) // <-- LÍNEA CORREGIDA
                   .ToListAsync();
        }

        // Método para OBTENER las reservas DE UN USUARIO (para el Cliente)
        public async Task<IEnumerable<Reserva>> GetReservasByUserIdAsync(int usuarioId)
        {
            return await _context.Reserva
                .Where(r => r.UsuarioId == usuarioId)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Cancha)
                .Include(r => r.Detalles)
                    .ThenInclude(d => d.Producto)
                     .OrderByDescending(r => r.FechaCreacion) // <-- LÍNEA CORREGIDA
                     .ToListAsync();
        }

        // Método para ACTUALIZAR EL ESTADO de una reserva (para el Admin)
        public async Task<Reserva> UpdateReservaStatusAsync(int id, string status)
        {
            var reserva = await _context.Reserva.FindAsync(id);
            if (reserva == null)
            {
                return null; // O lanzar una excepción si prefieres
            }

            reserva.Estado = status;
            await _context.SaveChangesAsync();
            return reserva;
        }

        // Método para ELIMINAR una reserva (para el Admin)
        public async Task<bool> DeleteReservaAsync(int id)
        {
            // Buscamos la reserva E incluimos sus detalles
            var reserva = await _context.Reserva
                .Include(r => r.Detalles)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (reserva == null)
            {
                return false; // No se encontró
            }

            // 1. Borramos los detalles asociados (sino la BD dará error de clave foránea)
            _context.ReservaDetalle.RemoveRange(reserva.Detalles);

            // 2. Borramos la reserva principal
            _context.Reserva.Remove(reserva);

            // 3. Guardamos todos los cambios
            await _context.SaveChangesAsync();
            return true;
        }
    }
}