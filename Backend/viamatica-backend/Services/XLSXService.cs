using System.Net;
using AutoMapper;
using OfficeOpenXml;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Utility;
using viamatica_backend.Repository;
using viamatica_backend.Tools;

namespace viamatica_backend.Services
{
    public class XLSXService
    {
        private readonly UsuarioService _usuarioService;
        private readonly IMapper _mapper;
        public XLSXService(UsuarioService usuarioService, IMapper mapper)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
        }

        public async Task<APIResponse<List<UsuarioDTO?>?>> SubirXLSXConUsuarios(IFormFile file)
        {
            try
            {
                var usuarios = await ExcelReader.ParseUsuariosDesdeExcelAsync(file);
                var usuariosCreados = new List<UsuarioDTO>();
                var errores = new List<string>();

                foreach (var usuario in usuarios)
                {
                    try
                    {
                        // Validaciones básicas
                        if (string.IsNullOrEmpty(usuario.Nombres) || string.IsNullOrEmpty(usuario.Apellidos))
                        {
                            errores.Add($"Nombre y apellido son obligatorios para {usuario.Nombres ?? "usuario desconocido"}.");
                            continue;
                        }

                        // Intentar guardar el usuario en la base de datos
                        var usuarioCreado = await _usuarioService.CrearUsuario(usuario);
                        if (usuarioCreado != null)
                        {
                            usuariosCreados.Add(_mapper.Map<UsuarioDTO>(usuarioCreado));
                        }
                    }
                    catch (Exception ex)
                    {
                        errores.Add($"Error con el usuario {usuario.Nombres ?? "Usuario desconocido"}: {ex.Message}");
                    }
                }

                // Construir la respuesta
                if (errores.Count > 0)
                {
                    return new APIResponse<List<UsuarioDTO?>?>(
                        usuariosCreados,
                        $"Usuarios creados con éxito: {usuariosCreados.Count}. Errores: {string.Join("; ", errores)}",
                        HttpStatusCode.PartialContent
                    );
                }

                return new APIResponse<List<UsuarioDTO?>?>(usuariosCreados, "Todos los usuarios fueron creados correctamente", HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return new APIResponse<List<UsuarioDTO?>?>(null, $"Error procesando el archivo: {ex.Message}", HttpStatusCode.InternalServerError);
            }
        }
    }
}
