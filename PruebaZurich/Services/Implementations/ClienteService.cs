using AutoMapper;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Models.DTOs.Clientes;
using PruebaZurich.Models.DTOs.Shared;
using PruebaZurich.Services.Interfaces;
using PruebaZurich.Exceptions;

namespace PruebaZurich.Services.Implementations
{
    public class ClienteService : IClienteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<ClienteService> _logger;

        public ClienteService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<ClienteService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<PagedResultDTO<ClienteDTO>> ListarClientes(ClienteFilterDTO filtro)
        {
            var clientes = await _unitOfWork.Clientes.FilterClientes(filtro);
            var total = await _unitOfWork.Clientes.CountAsync();

            return new PagedResultDTO<ClienteDTO>
            {
                Items = (List<ClienteDTO>)_mapper.Map<IEnumerable<ClienteDTO>>(clientes),
                TotalItems = total,
                PageNumber = filtro.PageNumber,
                PageSize = filtro.PageSize
            };
        }

        public async Task<ClienteDTO> ObtenerCliente(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado");

            return _mapper.Map<ClienteDTO>(cliente);
        }
        public async Task<ClienteDTO> ObtenerClientePorIdentificacion(string identificacion)
        {
            var cliente = await _unitOfWork.Clientes.GetByIdentificacion(identificacion);
            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado");

            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> CrearCliente(ClienteFormDTO clienteDto)
        {
            if (await _unitOfWork.Clientes.ExistsByIdentificacion(clienteDto.Identificacion))
                throw new BusinessException("La identificación ya está registrada");

            if (await _unitOfWork.Clientes.ExistsByEmail(clienteDto.Email))
                throw new BusinessException("El email ya está registrado");

            var cliente = _mapper.Map<Cliente>(clienteDto);
            await _unitOfWork.Clientes.Add(cliente);
            await _unitOfWork.Complete();

            _logger.LogInformation($"Cliente creado: {cliente.ClienteId}");
            return _mapper.Map<ClienteDTO>(cliente);
        }

        public async Task<ClienteDTO> ActualizarCliente(int id, ClienteFormDTO clienteDto)
        {
            var cliente = await _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado");

            _mapper.Map(clienteDto, cliente);
            cliente.FechaActualizacion = DateTime.UtcNow;
            _unitOfWork.Clientes.Update(cliente);
            await _unitOfWork.Complete();

            return _mapper.Map<ClienteDTO>(cliente);
        }
        public async Task EliminarCliente(int id)
        {
            var cliente = await _unitOfWork.Clientes.GetById(id);
            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado");

            // Verificar si el cliente tiene pólizas activas
            if (await _unitOfWork.Polizas.ClienteTienePolizasActivas(id))
                throw new BusinessException("No se puede eliminar un cliente con pólizas activas");

            _unitOfWork.Clientes.Remove(cliente);
            await _unitOfWork.Complete();

            _logger.LogInformation($"Cliente eliminado: {id}");
        }

        public async Task<bool> ExisteCliente(string identificacion)
        {
            return await _unitOfWork.Clientes.ExistsByIdentificacion(identificacion);
        }

    }
}
