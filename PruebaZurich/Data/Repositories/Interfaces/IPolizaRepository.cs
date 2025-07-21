using PruebaZurich.Data.Entities;
using PruebaZurich.Models.DTOs.Polizas;

namespace PruebaZurich.Data.Repositories.Interfaces
{
    public interface IPolizaRepository : IRepositoryBase<Poliza>
    {
        Task<IEnumerable<Poliza>> GetPolizasByCliente(int clienteId);
        Task<IEnumerable<Poliza>> FilterPolizas(PolizaFilterDTO filter);
        Task<bool> ClienteTienePolizasActivas(int clienteId);
        Task<int> CountAsync();
    }
}
