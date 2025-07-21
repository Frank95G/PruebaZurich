using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaZurich.Models.DTOs.Auth;
using PruebaZurich.Services.Interfaces;
using PruebaZurich.Exceptions;

namespace PruebaZurich.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IAuthService authService,
            ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        /// <summary>
        /// Inicia sesión en el sistema
        /// </summary>
        [AllowAnonymous]
        [HttpPost("login")]
        [ProducesResponseType(typeof(AuthResponseDTO), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                var response = await _authService.Login(loginDto);
                return Ok(response);
            }
            catch (UnauthorizedException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Registra un nuevo usuario (cliente)
        /// </summary>
        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(AuthResponseDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDTO registroDto)
        {
            try
            {
                var response = await _authService.Registrar(registroDto);
                return CreatedAtAction(nameof(Login), response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Registra un nuevo administrador (solo para superusuarios)
        /// </summary>
        [HttpPost("register/admin")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(AuthResponseDTO), 201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterDTO registroDto)
        {
            try
            {
                var response = await _authService.RegistrarAdministrador(registroDto);
                return CreatedAtAction(nameof(Login), response);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el registro de administrador");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }
    }
}