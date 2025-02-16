using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class RolOpcionesRepository : IBaseRepository<RolOpcione>
    {
        private readonly ViamaticaContext _context;

        public RolOpcionesRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolOpcione>> GetAllAsync()
        {
            return await _context.RolOpciones.ToListAsync();
        }

        public async Task<IEnumerable<RolOpcione>> GetFilteredAsync(Expression<Func<RolOpcione, bool>> filter)
        {
            return await _context.RolOpciones
                .Where(filter)
                .Include(x => x.IdRolNavigation)
                .ToListAsync();
        }

        public async Task<RolOpcione?> GetByIdAsync(int id)
        {
            return await _context.RolOpciones.FindAsync(id);
        }

        public async Task<RolOpcione> AddAsync(RolOpcione entity)
        {
            var addedEntity = await _context.RolOpciones.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<RolOpcione> UpdateAsync(RolOpcione entity)
        {
            var response = _context.RolOpciones.Update(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.RolOpciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<RolOpcione, bool>> predicate)
        {
            return await _context.RolOpciones.AnyAsync(predicate);
        }
    }
}
