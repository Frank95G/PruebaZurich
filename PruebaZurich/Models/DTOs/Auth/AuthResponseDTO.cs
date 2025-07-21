namespace PruebaZurich.Models.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }

    public class UsuarioDTO
    {
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Rol { get; set; }
        public int? ClienteId { get; set; }
    }
}
