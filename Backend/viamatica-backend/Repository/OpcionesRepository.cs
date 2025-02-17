using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class OpcionesRepository : IBaseRepository<Opcione>
    {
        private readonly ViamaticaContext _context;

        public OpcionesRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Opcione>> GetAllAsync()
        {
            return await _context.Opciones.ToListAsync();
        }

        public async Task<IEnumerable<Opcione>> GetFilteredAsync(Expression<Func<Opcione, bool>> filter)
        {
            return await _context.Opciones.Where(filter).ToListAsync();
        }

        public async Task<Opcione?> GetByIdAsync(int id)
        {
            return await _context.Opciones.FindAsync(id);
        }

        public async Task<Opcione> AddAsync(Opcione entity)
        {
            var addedOpcione = await _context.Opciones.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedOpcione.Entity;
        }

        public async Task<Opcione> UpdateAsync(Opcione entity)
        {
            var response = _context.Opciones.Update(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Opciones.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<Opcione, bool>> predicate)
        {
            return await _context.Opciones.AnyAsync(predicate);
        }
    }
}
