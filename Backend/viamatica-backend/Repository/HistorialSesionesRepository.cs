using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class HistorialSesioneRepository : IBaseRepository<HistorialSesione>
    {
        private readonly ViamaticaContext _context;

        public HistorialSesioneRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<HistorialSesione>> GetAllAsync()
        {
            return await _context.HistorialSesiones.ToListAsync();
        }

        public async Task<IEnumerable<HistorialSesione>> GetFilteredAsync(Expression<Func<HistorialSesione, bool>> filter)
        {
            return await _context.HistorialSesiones.Where(filter).ToListAsync();
        }

        public async Task<HistorialSesione?> GetByIdAsync(int id)
        {
            return await _context.HistorialSesiones.FindAsync(id);
        }

        public async Task<HistorialSesione> AddAsync(HistorialSesione entity)
        {
            var addedEntity = await _context.HistorialSesiones.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<HistorialSesione> UpdateAsync(HistorialSesione entity)
        {
            var response = _context.HistorialSesiones.Update(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.HistorialSesiones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<HistorialSesione, bool>> predicate)
        {
            return await _context.HistorialSesiones.AnyAsync(predicate);
        }
    }
}
