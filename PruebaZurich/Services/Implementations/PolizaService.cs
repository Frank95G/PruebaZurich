using AutoMapper;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Exceptions;
using PruebaZurich.Models.DTOs.Polizas;
using PruebaZurich.Models.DTOs.Shared;
using PruebaZurich.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PruebaZurich.Services.Implementations
{
    public class PolizaService : IPolizaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<PolizaService> _logger;

        public PolizaService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ILogger<PolizaService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }
        public async Task<PagedResultDTO<PolizaDTO>> ListarPolizas(PolizaFilterDTO filtro)
        {
            var polizas = await _unitOfWork.Polizas.FilterPolizas(filtro);
            var total = await _unitOfWork.Polizas.CountAsync();

            return new PagedResultDTO<PolizaDTO>
            {
                Items = (List<PolizaDTO>)_mapper.Map<IEnumerable<PolizaDTO>>(polizas),
                TotalItems = total,
                PageNumber = filtro.PageNumber,
                PageSize = filtro.PageSize
            };
        }


        public async Task<PolizaDTO> ObtenerPoliza(int id)
        {
            var poliza = await _unitOfWork.Polizas.GetById(id);
            if (poliza == null)
                throw new NotFoundException("Póliza no encontrada");

            return _mapper.Map<PolizaDTO>(poliza);
        }

        public async Task<IEnumerable<PolizaDTO>> ObtenerPolizasPorCliente(int clienteId)
        {
            var polizas = await _unitOfWork.Polizas.GetPolizasByCliente(clienteId);
            return _mapper.Map<IEnumerable<PolizaDTO>>(polizas);
        }

        public async Task<PolizaDTO> EmitirPoliza(PolizaFormDTO polizaDto)
        {
            var cliente = await _unitOfWork.Clientes.GetById(polizaDto.ClienteId);
            if (cliente == null)
                throw new NotFoundException("Cliente no encontrado");

            if (polizaDto.FechaExpiracion <= polizaDto.FechaInicio)
                throw new BusinessException("La fecha de expiración debe ser posterior a la fecha de inicio");

            var poliza = _mapper.Map<Poliza>(polizaDto);
            poliza.NumeroPoliza = GenerarNumeroPoliza();
            poliza.Estado = "Activa";

            //// Obtener el tipo de póliza
            //var tipoPoliza = poliza.TipoPoliza;
            //if (tipoPoliza == null)
            //    throw new NotFoundException("Tipo de póliza no encontrado");

            //poliza.TipoPoliza = tipoPoliza;

            await _unitOfWork.Polizas.Add(poliza);
            await _unitOfWork.Complete();

            _logger.LogInformation($"Póliza emitida: {poliza.NumeroPoliza}");
            return _mapper.Map<PolizaDTO>(poliza);
        }

        public async Task<PolizaDTO> ActualizarPoliza(int id, PolizaFormDTO polizaDto)
        {
            var poliza = await _unitOfWork.Polizas.GetById(id);
            if (poliza == null)
                throw new NotFoundException("Póliza no encontrada");

            if (polizaDto.FechaExpiracion <= polizaDto.FechaInicio)
            {
                throw new BusinessException("La fecha de expiración debe ser posterior a la fecha de inicio");
            }

            poliza.ClienteId = polizaDto.ClienteId;
            poliza.TipoPolizaId = polizaDto.TipoPolizaId;
            poliza.FechaInicio = polizaDto.FechaInicio;
            poliza.FechaExpiracion = polizaDto.FechaExpiracion;
            poliza.MontoAsegurado = polizaDto.MontoAsegurado;

            _unitOfWork.Polizas.Update(poliza);
            await _unitOfWork.Complete();

            return _mapper.Map<PolizaDTO>(poliza);
        }

        public async Task CancelarPoliza(int id, string motivo)
        {
            var poliza = await _unitOfWork.Polizas.GetById(id);
            if (poliza == null)
                throw new NotFoundException("Póliza no encontrada");

            if (poliza.Estado == "Cancelada")
                throw new BusinessException("La póliza ya está cancelada");

            poliza.Estado = "Cancelada";
            poliza.FechaCancelacion = DateTime.UtcNow;
            poliza.MotivoCancelacion = motivo;

            _unitOfWork.Polizas.Update(poliza);
            await _unitOfWork.Complete();

            _logger.LogInformation($"Póliza cancelada: {poliza.NumeroPoliza}");
        }
        public async Task RequestCancelarPoliza(int id)
        {
            var poliza = await _unitOfWork.Polizas.GetById(id);
            if (poliza == null)
                throw new NotFoundException("Póliza no encontrada");

            if (poliza.Estado == "Cancelada")
                throw new BusinessException("La póliza ya está cancelada");

            poliza.Estado = "Solicitud Cancelar";

            _unitOfWork.Polizas.Update(poliza);
            await _unitOfWork.Complete();

            _logger.LogInformation($"Póliza cancelada: {poliza.NumeroPoliza}");
        }
        
        public async Task<bool> ClienteTienePolizasActivas(int clienteId)
        {
            return await _unitOfWork.Polizas.ClienteTienePolizasActivas(clienteId);
        }

        private string GenerarNumeroPoliza()
        {
            return $"POL-{DateTime.Now:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 4).ToUpper()}";
        }

    }
}
