using PruebaZurich.Data.Entities;
using PruebaZurich.Models.DTOs.Auth;

namespace PruebaZurich.Services.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> Registrar(RegisterDTO registro);
        Task<AuthResponseDTO> RegistrarAdministrador(RegisterDTO registro);
        Task<AuthResponseDTO> Login(LoginDTO login);
        Task<bool> ExisteUsuario(string email);
        Task<AuthResponseDTO> GenerateAuthResponse(Usuario usuario, int? clienteId); 
    }
}
