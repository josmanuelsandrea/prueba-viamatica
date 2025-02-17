namespace viamatica_backend.Services.BackgroundJobs
{
    public class SessionExpirationChecker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public SessionExpirationChecker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var sesionesActivasService = scope.ServiceProvider.GetRequiredService<SesionesActivasService>();
                    var historialSesionesService = scope.ServiceProvider.GetRequiredService<HistorialSesionesService>();

                    // Obtener sesiones expiradas
                    var sesionesExpiradas = await sesionesActivasService.ObtenerSesionesExpiradas();

                    foreach (var sesion in sesionesExpiradas)
                    {
                        // Registrar el cierre en historial
                        await historialSesionesService.MarcarSesionComoCerrada(sesion.IdUsuario);

                        // Eliminar la sesión
                        await sesionesActivasService.EliminarSesion(sesion.IdUsuario);
                    }
                }

                // Esperar 5 minutos antes de la próxima verificación
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
