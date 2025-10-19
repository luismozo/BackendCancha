using BackendCancha.DTO;
using BackendCancha.Model;

namespace BackendCancha.Services
{
    public interface IReservaService
    {
        Task<Reserva> CreateReservaAsync(CreateReservaDTO dto, int usuarioId);
    }
}
