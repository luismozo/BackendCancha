using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;
using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Services
{
    public class CategoriaPService : ICategoriaPService
    {
        private readonly AppDBContext _context;

        public CategoriaPService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CategoriaP>> GetAllAsync()
        {
            return await _context.CategoriaP.ToListAsync();
        }

        public async Task<CategoriaP?> GetByIdAsync(int id)
        {
            return await _context.CategoriaP.FindAsync(id);
        }

        public async Task<CategoriaP> CreateAsync(CategoriaPDTO dto)
        {
            var categoria = new CategoriaP
            {
                Nombre = dto.Nombre
            };

            _context.CategoriaP.Add(categoria);
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<CategoriaP?> UpdateAsync(int id, CategoriaPDTO dto)
        {
            var categoria = await _context.CategoriaP.FindAsync(id);
            if (categoria == null)
            {
                return null;
            }

            categoria.Nombre = dto.Nombre;
            await _context.SaveChangesAsync();
            return categoria;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var categoria = await _context.CategoriaP.FindAsync(id);
            if (categoria == null)
            {
                return false;
            }

            _context.CategoriaP.Remove(categoria);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
