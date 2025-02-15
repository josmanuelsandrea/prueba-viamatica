using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class UsuarioRepository : IBaseRepository<Usuario>
    {
        private readonly ViamaticaContext _context;

        public UsuarioRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetFilteredAsync(Expression<Func<Usuario, bool>> filter)
        {
            return await _context.Usuarios.Where(filter).ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios.FindAsync(id);
        }

        public async Task<Usuario> AddAsync(Usuario entity)
        {
            entity.SessionActive = 'N';
            var addedUser = await _context.Usuarios.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedUser.Entity;
        }

        public async Task UpdateAsync(Usuario entity)
        {
            _context.Usuarios.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Usuarios.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<Usuario, bool>> predicate)
        {
            return await _context.Usuarios.AnyAsync(predicate);
        }
    }
}
