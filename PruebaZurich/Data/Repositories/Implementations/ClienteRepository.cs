using Microsoft.EntityFrameworkCore;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Models.DTOs.Clientes;

namespace PruebaZurich.Data.Repositories.Implementations
{
    public class ClienteRepository : RepositoryBase<Cliente>, IClienteRepository
    {
        public ClienteRepository(ZurichDBContext context) : base(context) { }

        public async Task<Cliente> GetByIdentificacion(string identificacion)
        {
            return await _context.Clientes
                .Include(c => c.Polizas)
                .FirstOrDefaultAsync(c => c.Identificacion == identificacion);
        }
        public async Task<Cliente> GetByEmail(string email)
        {
            return await _context.Clientes
                .Include(c => c.Polizas)
                .FirstOrDefaultAsync(c => c.Email == email);
        }
        public async Task<Cliente> GetByUsuarioId(int usuarioId)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
        }

        public async Task<IEnumerable<Cliente>> FilterClientes(ClienteFilterDTO filter)
        {
            var query = _context.Clientes
                .Include(c => c.Polizas)
                .AsQueryable();

            //if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            //{
            //    query = query.Where(c =>
            //        c.Nombre.Contains(filter.SearchTerm) ||
            //        c.Email.Contains(filter.SearchTerm) ||
            //        c.Identificacion.Contains(filter.SearchTerm));
            //}

            if (!string.IsNullOrWhiteSpace(filter.Identificacion))
            {
                query = query.Where(c => c.Identificacion == filter.Identificacion);
            }

            if (!string.IsNullOrWhiteSpace(filter.Email))
            {
                query = query.Where(c => c.Email == filter.Email);
            }
            if (!string.IsNullOrWhiteSpace(filter.Nombre))
            {
                query = query.Where(c => c.Nombre.Contains(filter.Nombre));
            }

            if (!string.IsNullOrWhiteSpace(filter.Telefono))
            {
                query = query.Where(c => c.Telefono == filter.Telefono);
            }

            if (!string.IsNullOrWhiteSpace(filter.Direccion))
            {
                query = query.Where(c => c.Direccion.Contains(filter.Direccion));
            }

            return await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<bool> ExistsByIdentificacion(string identificacion)
        {
            return await _context.Clientes.AnyAsync(c => c.Identificacion == identificacion);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Clientes.AnyAsync(c => c.Email == email);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Clientes.CountAsync();
        }        
    }
}
