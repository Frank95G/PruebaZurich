using System.ComponentModel.DataAnnotations;

namespace PruebaZurich.Models.DTOs.Auth
{
    public class LoginDTO
    {
            [Required(ErrorMessage = "El username es obligatorio")]
            public string Username { get; set; }

            [Required(ErrorMessage = "La contraseña es obligatoria")]
            public string Password { get; set; }
    }
}
