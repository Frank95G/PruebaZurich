using Microsoft.EntityFrameworkCore;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;
using PruebaZurich.Models.DTOs.Polizas;

namespace PruebaZurich.Data.Repositories.Implementations
{
    public class PolizaRepository : RepositoryBase<Poliza>, IPolizaRepository
    {
        public PolizaRepository(ZurichDBContext context) : base(context) { }

        public async Task<IEnumerable<Poliza>> GetPolizasByCliente(int clienteId)
        {
            return await _context.Polizas
                .Include(p => p.TipoPoliza)
                .Where(p => p.ClienteId == clienteId)
                .OrderByDescending(p => p.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Poliza>> FilterPolizas(PolizaFilterDTO filter)
        {
            var query = _context.Polizas
                .Include(p => p.TipoPoliza)
                .Include(p => p.Cliente)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(filter.NumeroPoliza))
            {
                query = query.Where(p => p.NumeroPoliza == filter.NumeroPoliza);
            }
            if (filter.ClienteId.HasValue)
            {
                query = query.Where(p => p.ClienteId == filter.ClienteId.Value);
            }

            if (filter.TipoPolizaId.HasValue)
            {
                query = query.Where(p => p.TipoPolizaId == filter.TipoPolizaId.Value);
            }

            if (!string.IsNullOrWhiteSpace(filter.Estado))
            {
                query = query.Where(p => p.Estado == filter.Estado);
            }

            if (filter.FechaDesde.HasValue)
            {
                query = query.Where(p => p.FechaInicio >= filter.FechaDesde.Value);
            }

            if (filter.FechaHasta.HasValue)
            {
                query = query.Where(p => p.FechaInicio <= filter.FechaHasta.Value);
            }

            return await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();
        }

        public async Task<bool> ClienteTienePolizasActivas(int clienteId)
        {
            return await _context.Polizas
                .AnyAsync(p => p.ClienteId == clienteId && p.Estado == "Activa");
        }

        public async Task<int> CountAsync()
        {
            return await _context.Polizas.CountAsync();
        }

    }
}
