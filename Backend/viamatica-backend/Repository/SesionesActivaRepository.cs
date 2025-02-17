using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class SesionesActivaRepository : IBaseRepository<SesionesActiva>
    {
        private readonly ViamaticaContext _context;

        public SesionesActivaRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<SesionesActiva>> GetAllAsync()
        {
            return await _context.SesionesActivas.ToListAsync();
        }

        public async Task<IEnumerable<SesionesActiva>> GetFilteredAsync(Expression<Func<SesionesActiva, bool>> filter)
        {
            return await _context.SesionesActivas.Where(filter)
                .Include(x => x.IdUsuarioNavigation)
                    .ThenInclude(x => x.IdPersonaNavigation)
                .ToListAsync();
        }

        public async Task<SesionesActiva?> GetByIdAsync(int id)
        {
            return await _context.SesionesActivas.FindAsync(id);
        }

        public async Task<SesionesActiva> AddAsync(SesionesActiva entity)
        {
            var addedEntity = await _context.SesionesActivas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedEntity.Entity;
        }

        public async Task<SesionesActiva> UpdateAsync(SesionesActiva entity)
        {
            var response = _context.SesionesActivas.Update(entity);
            await _context.SaveChangesAsync();
            return response.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.SesionesActivas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<SesionesActiva, bool>> predicate)
        {
            return await _context.SesionesActivas.AnyAsync(predicate);
        }
    }
}
