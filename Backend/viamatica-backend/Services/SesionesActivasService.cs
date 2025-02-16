using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using viamatica_backend.DBModels;
using viamatica_backend.Repository;

namespace viamatica_backend.Services
{
    public class SesionesActivasService
    {
        private readonly SesionesActivaRepository _sesionesActivaRepository;
        public SesionesActivasService(SesionesActivaRepository sesionesActivaRepository)
        {
            _sesionesActivaRepository = sesionesActivaRepository;
        }

        public async Task<SesionesActiva> RegistrarSesionActiva(int userId, string token)
        {
            // Verificar si el usuario ya tiene una sesión activa
            var sesiones = await _sesionesActivaRepository.GetFilteredAsync(s => s.IdUsuario == userId);
            var sesionExistente = sesiones.FirstOrDefault();

            if (sesionExistente != null)
            {
                // Eliminar sesión anterior para evitar múltiples sesiones activas por usuario
                await _sesionesActivaRepository.DeleteAsync(sesionExistente.IdSesion);
            }

            // Crear nueva sesión activa
            var nuevaSesion = new SesionesActiva
            {
                IdUsuario = userId,
                Token = token,
                FechaExpiracion = DateTime.UtcNow.AddHours(2) // Expira en 2 horas
            };

            // Guardar en la base de datos
            var result = await _sesionesActivaRepository.AddAsync(nuevaSesion);
            return result;
        }
        public async Task<Usuario?> ObtenerUsuarioPorToken(string token)
        {
            var sessions = await _sesionesActivaRepository.GetFilteredAsync(s => s.Token == token);
            var foundSession = sessions.FirstOrDefault();

            if (foundSession == null)
            {
                return null;
            }

            if (foundSession == null || foundSession.FechaExpiracion < DateTime.UtcNow)
            {
                return null; // Token inválido o expirado
            }

            var foundUser = foundSession.IdUsuarioNavigation;
            return foundUser;
        }

        public async Task<SesionesActiva?> ObtenerSesionPorToken(string token)
        {
            var sessions = await _sesionesActivaRepository.GetFilteredAsync(s => s.Token == token);
            var foundSession = sessions.FirstOrDefault();

            if (foundSession == null || foundSession.FechaExpiracion < DateTime.UtcNow)
            {
                return null; // Token inválido o expirado
            }

            return foundSession;
        }

        public async Task<bool> EliminarSesion(int id)
        {
            try
            {
                await _sesionesActivaRepository.DeleteAsync(id);
                return true;
            } catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
