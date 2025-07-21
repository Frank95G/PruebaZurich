using PruebaZurich.Data.Entities;
using PruebaZurich.Models.DTOs.Clientes;

namespace PruebaZurich.Data.Repositories.Interfaces
{
    public interface IClienteRepository : IRepositoryBase<Cliente>
    {
        Task<Cliente> GetByIdentificacion(string identificacion);
        Task<Cliente> GetByEmail(string email);
        Task<Cliente> GetByUsuarioId(int usuarioId);
        Task<IEnumerable<Cliente>> FilterClientes(ClienteFilterDTO filter);
        Task<bool> ExistsByIdentificacion(string identificacion);
        Task<bool> ExistsByEmail(string email);
        Task<int> CountAsync();
    }
}
