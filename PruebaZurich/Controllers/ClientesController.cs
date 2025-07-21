using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PruebaZurich.Models.DTOs.Clientes;
using PruebaZurich.Models.DTOs.Shared;
using PruebaZurich.Services.Interfaces;
using PruebaZurich.Exceptions;

namespace PruebaZurich.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private readonly ILogger<ClientesController> _logger;

        public ClientesController(
            IClienteService clienteService,
            ILogger<ClientesController> logger)
        {
            _clienteService = clienteService;
            _logger = logger;
        }

        /// <summary>
        /// Obtiene una lista paginada de clientes
        /// </summary>
        [HttpGet]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(PagedResultDTO<ClienteDTO>), 200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(403)]
        public async Task<IActionResult> GetAll([FromQuery] ClienteFilterDTO filtro)
        {
            try
            {
                var clientes = await _clienteService.ListarClientes(filtro);
                return Ok(clientes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener clientes");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Obtiene un cliente por su ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClienteDTO), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var cliente = await _clienteService.ObtenerCliente(id);
                return Ok(cliente);
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al obtener cliente con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(typeof(ClienteDTO), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Create([FromBody] ClienteFormDTO clienteDto)
        {
            try
            {
                var nuevoCliente = await _clienteService.CrearCliente(clienteDto);
                return CreatedAtAction(nameof(GetById), new { id = nuevoCliente.ClienteId }, nuevoCliente);
            }
            catch (BusinessException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear cliente");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(ClienteDTO), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Update(int id, [FromBody] ClienteFormDTO clienteDto)
        {
            try
            {
                var clienteActualizado = await _clienteService.ActualizarCliente(id, clienteDto);
                return Ok(clienteActualizado);
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
                _logger.LogError(ex, $"Error al actualizar cliente con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }

        /// <summary>
        /// Elimina un cliente
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _clienteService.EliminarCliente(id);
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
                _logger.LogError(ex, $"Error al eliminar cliente con ID {id}");
                return StatusCode(500, "Ocurrió un error interno");
            }
        }
    }
}