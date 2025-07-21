using PruebaZurich.Models.DTOs.Polizas;
using PruebaZurich.Models.DTOs.Shared;

namespace PruebaZurich.Services.Interfaces
{
    public interface IPolizaService
    {
        Task<PagedResultDTO<PolizaDTO>> ListarPolizas(PolizaFilterDTO filtro);
        Task<PolizaDTO> ObtenerPoliza(int id);
        Task<IEnumerable<PolizaDTO>> ObtenerPolizasPorCliente(int clienteId);
        Task<PolizaDTO> EmitirPoliza(PolizaFormDTO poliza);
        Task<PolizaDTO> ActualizarPoliza(int id, PolizaFormDTO poliza);
        Task CancelarPoliza(int id, string motivo);
        Task RequestCancelarPoliza(int id);
        Task<bool> ClienteTienePolizasActivas(int clienteId);
    }
}
