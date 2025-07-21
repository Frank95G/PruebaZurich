using Microsoft.EntityFrameworkCore.Storage;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Repositories.Interfaces;
using System.Data;

namespace PruebaZurich.Data.Repositories.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ZurichDBContext _context;
        private IDbContextTransaction _transaction;

        public UnitOfWork(ZurichDBContext context)
        {
            _context = context;
            Clientes = new ClienteRepository(_context);
            Polizas = new PolizaRepository(_context);
            Usuarios = new UsuarioRepository(_context);
        }

        public IClienteRepository Clientes { get; }
        public IPolizaRepository Polizas { get; }
        public IUsuarioRepository Usuarios { get; }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbTransaction> BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
            return _transaction.GetDbTransaction();
        }

        public async Task CommitTransactionAsync(IDbTransaction transaction)
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay transacción activa");

            await _transaction.CommitAsync();
        }

        public async Task RollbackTransactionAsync(IDbTransaction transaction)
        {
            if (_transaction == null)
                throw new InvalidOperationException("No hay transacción activa");

            await _transaction.RollbackAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _context.Dispose();
            }
        }
    }
}
