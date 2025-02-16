using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using viamatica_backend.DTOS;
using viamatica_backend.Models.Request;
using viamatica_backend.Services;

namespace viamatica_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<LoginResponseDTO> Login([FromBody] UserLoginData data)
        {
            var result = await _authService.Login(data);
            return result;
        }

        [HttpPost("whoami")]
        public async Task<ActionResult<LoginResponseDTO>> WhoAmI([FromBody] TokenRequest token)
        {
            var result = await _authService.WhoAmI(token);
            return StatusCode((int)result.StatusCode, result);
        }

        [HttpPost("logout")]
        public async Task<ActionResult<bool>> Login([FromBody] TokenRequest token)
        {
            var result = await _authService.Logout(token);
            return StatusCode((int)result.StatusCode, result.Data);
        }
    }
}
