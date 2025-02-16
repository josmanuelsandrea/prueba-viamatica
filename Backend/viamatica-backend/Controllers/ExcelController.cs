using System.ComponentModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using viamatica_backend.Services;

namespace viamatica_backend.Controllers
{
    [Route("api/upload/xlsx")]
    [ApiController]
    public class ExcelController : ControllerBase
    {
        private readonly XLSXService _xlsxService;
        public ExcelController(UsuarioService usuarioService, XLSXService xlsxService)
        {
            _xlsxService = xlsxService;
        }

        [HttpPost]
        public async Task<ActionResult> SubirXLSXConUsuarios(IFormFile file)
        {
            var usuariosSubidos = await _xlsxService.SubirXLSXConUsuarios(file);
            return StatusCode((int)usuariosSubidos.StatusCode, usuariosSubidos);
        }
    }
}
