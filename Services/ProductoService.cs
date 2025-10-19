using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Services
{
    public class ProductoService : IProductoService
    {
        private readonly AppDBContext _context;

        public ProductoService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Productos>> GetAllAsync()
        {
            // Usamos Include para cargar la información de la categoría relacionada.
            // Esto es crucial para que el frontend pueda mostrar el nombre de la categoría.
            return await _context.Productos.Include(p => p.CategoriaP).ToListAsync();
        }

        public async Task<Productos?> GetByIdAsync(int id)
        {
            return await _context.Productos.Include(p => p.CategoriaP).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Productos> CreateAsync(ProductosDTO dto)
        {
            var producto = new Productos
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Cantidad = dto.Cantidad,
                Precio = dto.Precio,
                ImagenUrl = dto.ImagenUrl,
                CategoriaPId = dto.CategoriaPId
            };

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<Productos?> UpdateAsync(int id, ProductosDTO dto)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return null; // El controlador devolverá un 404 Not Found.
            }

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Cantidad = dto.Cantidad;
            producto.Precio = dto.Precio;
            producto.ImagenUrl = dto.ImagenUrl;
            producto.CategoriaPId = dto.CategoriaPId;

            await _context.SaveChangesAsync();
            return producto;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return false; // El controlador devolverá un 404 Not Found.
            }

            _context.Productos.Remove(producto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
