using PruebaZurich.Models.DTOs.Auth;
using PruebaZurich.Models.DTOs.Shared;

namespace PruebaZurich.Services.Interfaces
{
    public interface IUsuarioService
    {
        Task<AuthResponseDTO> RegistrarAdministrador(RegisterDTO registro);
        Task<AuthResponseDTO> RegistrarCliente(RegisterDTO registro);
        Task<PagedResultDTO<UsuarioDTO>> ListarUsuarios(int pageNumber = 1, int pageSize = 10);
        Task<UsuarioDTO> ObtenerUsuario(int id);
        Task AsignarRol(int usuarioId, string rol);
    }
}
