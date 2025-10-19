using BackendCancha.Data;
using BackendCancha.DTO;
using BackendCancha.Model;

using Microsoft.EntityFrameworkCore;

namespace BackendCancha.Services
{
    public class CanchaService : ICanchaService
    {
        private readonly AppDBContext _context;

        public CanchaService(AppDBContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Canchas>> GetAllAsync()
        {
            return await _context.Canchas.ToListAsync();
        }

        public async Task<Canchas?> GetByIdAsync(int id)
        {
            return await _context.Canchas.FindAsync(id);
        }

        public async Task<Canchas> CreateAsync(CanchasDTO dto)
        {
            var cancha = new Canchas
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                AlquilerporHoras = dto.AlquilerporHoras,
                Estado = dto.Estado,
                ImagenUrl = dto.ImagenUrl,
                Fechaasintencia = dto.Fechaasintencia
            };

            _context.Canchas.Add(cancha);
            await _context.SaveChangesAsync();
            return cancha;
        }

        public async Task<Canchas?> UpdateAsync(int id, CanchasDTO dto)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null)
            {
                return null;
            }

            cancha.Nombre = dto.Nombre;
            cancha.Descripcion = dto.Descripcion;
            cancha.AlquilerporHoras = dto.AlquilerporHoras;
            cancha.Estado = dto.Estado;
            cancha.ImagenUrl = dto.ImagenUrl;
            cancha.Fechaasintencia = dto.Fechaasintencia;

            await _context.SaveChangesAsync();
            return cancha;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cancha = await _context.Canchas.FindAsync(id);
            if (cancha == null)
            {
                return false;
            }

            _context.Canchas.Remove(cancha);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
