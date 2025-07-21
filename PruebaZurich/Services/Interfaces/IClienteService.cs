using PruebaZurich.Models.DTOs.Clientes;
using PruebaZurich.Models.DTOs.Shared;

namespace PruebaZurich.Services.Interfaces
{
    public interface IClienteService
    {
        Task<PagedResultDTO<ClienteDTO>> ListarClientes(ClienteFilterDTO filtro);
        Task<ClienteDTO> ObtenerCliente(int id);
        Task<ClienteDTO> ObtenerClientePorIdentificacion(string identificacion);
        Task<ClienteDTO> CrearCliente(ClienteFormDTO cliente);
        Task<ClienteDTO> ActualizarCliente(int id, ClienteFormDTO cliente);
        Task EliminarCliente(int id);
        Task<bool> ExisteCliente(string identificacion);
    }
}
