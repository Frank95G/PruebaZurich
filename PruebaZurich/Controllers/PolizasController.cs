using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaZurich.Exceptions;
using PruebaZurich.Models.DTOs.Clientes;
using PruebaZurich.Models.DTOs.Polizas;
using PruebaZurich.Models.DTOs.Shared;
using PruebaZurich.Services.Interfaces;
using System.Security.Claims;

namespace PruebaZurich.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PolizasController : ControllerBase
    {
        private readonly IPolizaService _polizaService;
        private readonly ILogger<PolizasController> _logger;

        public PolizasController(
            IPolizaService polizaService,
            ILogger<PolizasController> logger)
        {
            _polizaService = polizaService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene todas las pólizas con filtros
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResultDTO<PolizaDTO>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PolizaFilterDTO filtro)
        {
            try
            {
                var polizas = await _polizaService.ListarPolizas(filtro);
                return Ok(polizas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener pólizas");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Obtiene una póliza por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PolizaDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var poliza = await _polizaService.ObtenerPoliza(id);
                return Ok(poliza);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener póliza con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Obtiene las pólizas de un cliente específico
        /// </summary>
        [HttpGet("cliente/{clienteId}")]
        [ProducesResponseType(typeof(IEnumerable<PolizaDTO>), 200)]
        public async Task<IActionResult> GetByCliente(int clienteId)
        {
            try
            {
                // Verificar si el usuario es el cliente o administrador
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var isAdmin = User.IsInRole("Administrador");

                if (!isAdmin && userId != clienteId.ToString())
                    return Forbid();

                var polizas = await _polizaService.ObtenerPolizasPorCliente(clienteId);
                return Ok(polizas);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener pólizas del cliente {clienteId}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Emite una nueva póliza
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(PolizaDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] PolizaFormDTO polizaDto)
        {
            try
            {
                var nuevaPoliza = await _polizaService.EmitirPoliza(polizaDto);
                return CreatedAtAction(nameof(GetById), new { id = nuevaPoliza.PolizaId }, nuevaPoliza);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al emitir póliza");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Actualiza una póliza existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(PolizaDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Update(int id, [FromBody] PolizaFormDTO polizaDto)
        {
            try
            {
                var polizaActualizada = await _polizaService.ActualizarPoliza(id, polizaDto);
                return Ok(polizaActualizada);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al actualizar póliza con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Cancela una póliza
        /// </summary>
        [HttpPost("{id}/cancelar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Cancel(int id, [FromBody] CancelarPolizaDTO cancelacionDto)
        {
            try
            {
                await _polizaService.CancelarPoliza(id, cancelacionDto.Motivo);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cancelar póliza con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Cancela una póliza
        /// </summary>
        [HttpPost("{id}/solcancelar")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> RequestCancel(int id)
        {
            try
            {
                await _polizaService.RequestCancelarPoliza(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cancelar póliza con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }
    }
}