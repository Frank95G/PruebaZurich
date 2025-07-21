using PruebaZurich.Data.Entities;

namespace PruebaZurich.Data.Repositories.Interfaces
{
    public interface IUsuarioRepository : IRepositoryBase<Usuario>
    {
        Task<Usuario> GetByEmail(string email);
        Task<Usuario> GetByUser(string email);
        Task<bool> ExistsByEmail(string email);
        Task<int> CountAsync();
    }
}
