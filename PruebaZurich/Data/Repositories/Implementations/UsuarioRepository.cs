using Microsoft.EntityFrameworkCore;
using PruebaZurich.Data.Context;
using PruebaZurich.Data.Entities;
using PruebaZurich.Data.Repositories.Interfaces;

namespace PruebaZurich.Data.Repositories.Implementations
{
    public class UsuarioRepository : RepositoryBase<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ZurichDBContext context) : base(context) { }

        public async Task<Usuario> GetByEmail(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }
        public async Task<Usuario> GetByUser(string username)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.NombreUsuario == username);
        }

        public async Task<bool> ExistsByEmail(string email)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.Email == email);
        }

        public async Task<int> CountAsync()
        {
            return await _context.Usuarios.CountAsync();
        }
    }
}
