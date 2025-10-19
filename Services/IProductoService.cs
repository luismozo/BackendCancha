using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;

namespace BackendCancha.Services
{
    public interface IProductoService
    {
        // Contrato para obtener todos los productos, incluyendo su categoría.
        Task<IEnumerable<Productos>> GetAllAsync();

        // Contrato para obtener un producto por su ID, incluyendo su categoría.
        Task<Productos?> GetByIdAsync(int id);

        // Contrato para crear un nuevo producto.
        Task<Productos> CreateAsync(ProductosDTO dto);

        // Contrato para actualizar un producto existente.
        Task<Productos?> UpdateAsync(int id, ProductosDTO dto);

        // Contrato para eliminar un producto.
        Task<bool> DeleteAsync(int id);
    }
}
