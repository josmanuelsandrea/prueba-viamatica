using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using viamatica_backend.DBModels;
using viamatica_backend.Interfaces;

namespace viamatica_backend.Repository
{
    public class PersonaRepository : IBaseRepository<Persona>
    {
        private readonly ViamaticaContext _context;

        public PersonaRepository(ViamaticaContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Persona>> GetAllAsync()
        {
            return await _context.Personas.ToListAsync();
        }

        public async Task<IEnumerable<Persona>> GetFilteredAsync(Expression<Func<Persona, bool>> filter)
        {
            return await _context.Personas.Where(filter).ToListAsync();
        }

        public async Task<Persona?> GetByIdAsync(int id)
        {
            return await _context.Personas.FindAsync(id);
        }

        public async Task<Persona> AddAsync(Persona entity)
        {
            var addedPersona = await _context.Personas.AddAsync(entity);
            await _context.SaveChangesAsync();
            return addedPersona.Entity;
        }

        public async Task<Persona> UpdateAsync(Persona entity)
        {
            var updatedData = _context.Personas.Update(entity);
            await _context.SaveChangesAsync();
            return updatedData.Entity;
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _context.Personas.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(Expression<Func<Persona, bool>> predicate)
        {
            return await _context.Personas.AnyAsync(predicate);
        }
    }
}
