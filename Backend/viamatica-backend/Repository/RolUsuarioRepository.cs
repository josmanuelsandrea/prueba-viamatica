using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class RolUsuarioRepository : IBaseRepository<RolUsuario>
    {
        private readonly ViamaticaContext _context;

        public RolUsuarioRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RolUsuario>> GetAllAsync()
        {
            return await _context.RolUsuarios.ToListAsync();
        }

        public async Task<IEnumerable<RolUsuario>> GetFilteredAsync(Expression<Func<RolUsuario, bool>> filter)
        {
            return await _context.RolUsuarios
                .Where(filter)
                .Include(x => x.IdRolNavigation)
                .Include(x => x.IdUsuarioNavigation)
                    .ThenInclude(x => x.IdPersonaNavigation)
                .ToListAsync();
        }

        public async Task<RolUsuario?> GetByIdAsync(int id)
        {
            return await _context.RolUsuarios.FindAsync(id);
        }

        public async Task<RolUsuario> AddAsync(RolUsuario entity)
        {
            var addedEntity = await _context.RolUsuarios.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task UpdateAsync(RolUsuario entity)
        {
            _context.RolUsuarios.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.RolUsuarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<RolUsuario, bool>> predicate)
        {
            return await _context.RolUsuarios.AnyAsync(predicate);
        }
    }
}
