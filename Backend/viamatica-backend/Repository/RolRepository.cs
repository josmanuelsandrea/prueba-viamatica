using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class RolRepository : IBaseRepository<Rol>
    {
        private readonly ViamaticaContext _context;

        public RolRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rol>> GetAllAsync()
        {
            return await _context.Rols.ToListAsync();
        }

        public async Task<IEnumerable<Rol>> GetFilteredAsync(Expression<Func<Rol, bool>> filter)
        {
            return await _context.Rols.Where(filter).ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Rols.FindAsync(id);
        }

        public async Task<Rol> AddAsync(Rol entity)
        {
            var addedRol = await _context.Rols.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedRol.Entity;
        }

        public async Task UpdateAsync(Rol entity)
        {
            _context.Rols.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Rols.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<Rol, bool>> predicate)
        {
            return await _context.Rols.AnyAsync(predicate);
        }
    }
}
