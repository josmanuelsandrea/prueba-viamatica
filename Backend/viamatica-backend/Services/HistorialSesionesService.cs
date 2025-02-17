using viamatica_backend.DBModels;
using viamatica_backend.Repository;

namespace viamatica_backend.Services
{
    public class HistorialSesionesService
    {
        private readonly HistorialSesioneRepository _historialSesioneRepository;
        public HistorialSesionesService(HistorialSesioneRepository historialSesioneRepository)
        {
            _historialSesioneRepository = historialSesioneRepository;
        }
        public async Task RegistrarHistorial(int userId, bool exito, string? token)
        {
            var nuevoHistorial = new HistorialSesione
            {
                IdUsuario = userId,
                FechaInicio = DateTime.UtcNow,
                Exito = exito,
                Token = exito ? token : null
            };

            await _historialSesioneRepository.AddAsync(nuevoHistorial);
        }

        public async Task<IEnumerable<HistorialSesione>> ObtenerHistorialPorUsuario(int userId)
        {
            return await _historialSesioneRepository.GetFilteredAsync(h => h.IdUsuario == userId);
        }

        public async Task MarcarSesionComoCerrada(int userId)
        {
            var historialSesion = await _historialSesioneRepository.GetFilteredAsync(h => h.IdUsuario == userId && h.FechaCierre == null);

            foreach (var sesion in historialSesion)
            {
                sesion.FechaCierre = DateTime.UtcNow;
                await _historialSesioneRepository.UpdateAsync(sesion);
            }
        }
    }
}
