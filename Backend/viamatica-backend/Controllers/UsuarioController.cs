using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.WebSockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;
using viamatica_backend.Models.Utility;
using viamatica_backend.Services;

namespace viamatica_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly UsuarioService _usuarioService;
        private readonly PersonService _personService;

        public UsuarioController(UsuarioService usuarioService, PersonService personService)
        {
            _usuarioService = usuarioService;
            _personService = personService;
        }

        [HttpGet("common")]
        public async Task<ActionResult<APIResponse<IEnumerable<UsuarioDTO>>>> ObtenerUsuariosComunes()
        {
            var result = await _usuarioService.ObtenerUsuariosComunes();
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost]
        public async Task<ActionResult<APIResponse<dynamic>>> CrearUsuario([FromBody] PersonaRequest persona)
        {
            var context = new ValidationContext(persona, serviceProvider: null, items: null);
            var results = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(persona, context, results, true);

            if (!isValid)
            {
                var errors = new { Errores = results.Select(e => e.ErrorMessage) };
                return new APIResponse<dynamic>(errors, "Existen errores de validacion en los datos recibidos", HttpStatusCode.BadRequest);
            }

            var result = await _usuarioService.CrearUsuario(persona);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPut("updatePerson")]
        public async Task<ActionResult<APIResponse<UsuarioDTO>>> ActualizarUsuario(PersonaUpdateData data)
        {
            var result = await _personService.UpdatePersonData(data);
            return StatusCode((int)result.StatusCode, result);
        }
    }
}
