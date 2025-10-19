using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;

namespace BackendCancha.Services
{
    public interface ICanchaService
    {
        Task<IEnumerable<Canchas>> GetAllAsync();
        Task<Canchas?> GetByIdAsync(int id);
        Task<Canchas> CreateAsync(CanchasDTO dto);
        Task<Canchas?> UpdateAsync(int id, CanchasDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
