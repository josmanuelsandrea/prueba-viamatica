using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
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
            return await _context.Rols
                .FromSqlRaw("SELECT * FROM sp_obtener_roles()")
                .ToListAsync();
        }

        public async Task<IEnumerable<Rol>> GetFilteredAsync(Expression<Func<Rol, bool>> filter)
        {
            return await _context.Rols.Where(filter).ToListAsync();
        }

        public async Task<Rol?> GetByIdAsync(int id)
        {
            return await _context.Rols
                .FromSqlRaw("SELECT * FROM sp_obtener_rol_por_id({0})", id)
                .FirstOrDefaultAsync();
        }

        public async Task<Rol> AddAsync(Rol entity)
        {
            var newId = await _context.Database
                .ExecuteSqlRawAsync("SELECT sp_crear_rol({0})", entity.NombreRol);

            entity.IdRol = newId;
            return entity;
        }

        public async Task<Rol> UpdateAsync(Rol entity)
        {
            await _context.Database
                .ExecuteSqlRawAsync("SELECT sp_actualizar_rol({0}, {1})", entity.IdRol, entity.NombreRol);

            return entity;
        }

        public async Task DeleteAsync(int id)
        {
            await _context.Database
                .ExecuteSqlRawAsync("SELECT sp_eliminar_rol({0})", id);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Rol, bool>> predicate)
        {
            return await _context.Rols.AnyAsync(predicate);
        }
    }
}
