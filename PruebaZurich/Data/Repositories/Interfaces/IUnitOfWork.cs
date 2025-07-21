using PruebaZurich.Data.Repositories.Implementations;
using System.Data;

namespace PruebaZurich.Data.Repositories.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IClienteRepository Clientes { get; }
        IPolizaRepository Polizas { get; }
        IUsuarioRepository Usuarios { get; }

        Task<int> Complete();
        Task<IDbTransaction> BeginTransactionAsync();
        Task CommitTransactionAsync(IDbTransaction transaction);
        Task RollbackTransactionAsync(IDbTransaction transaction);
    }
}
