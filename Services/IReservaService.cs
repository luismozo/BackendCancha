using BackendCancha.DTO;
using BackendCancha.Model;

namespace BackendCancha.Services
{
    public interface IReservaService
    {
        // Tu método existente
        Task<Reserva> CreateReservaAsync(CreateReservaDTO dto, int usuarioId);

        // --- MÉTODOS AÑADIDOS ---

        // Para la vista del Admin: Obtiene TODAS las reservas
        Task<IEnumerable<Reserva>> GetAllReservasAsync();

        // Para la vista del Cliente: Obtiene solo las de un usuario
        Task<IEnumerable<Reserva>> GetReservasByUserIdAsync(int usuarioId);

        // Para los botones del Admin: Actualiza el estado (ej: "Confirmado", "Cancelado")
        Task<Reserva> UpdateReservaStatusAsync(int id, string status);

        // Para el botón de eliminar del Admin
        Task<bool> DeleteReservaAsync(int id);
    }
}