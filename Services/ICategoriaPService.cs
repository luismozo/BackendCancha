using BackendCancha.DTO;
using BackendCancha.Model;

namespace BackendCancha.Services
{
    public interface ICategoriaPService
    {
        Task<IEnumerable<CategoriaP>> GetAllAsync();
        Task<CategoriaP?> GetByIdAsync(int id);
        Task<CategoriaP> CreateAsync(CategoriaPDTO dto);
        Task<CategoriaP?> UpdateAsync(int id, CategoriaPDTO dto);
        Task<bool> DeleteAsync(int id);
    }
}
