using System.ComponentModel.DataAnnotations;

namespace PruebaZurich.Models.DTOs.Clientes
{
    public class ClienteFormDTO
    {
        [Required(ErrorMessage = "La identificación es obligatoria")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "La identificación debe tener 10 dígitos")]
        [RegularExpression(@"^\d+$", ErrorMessage = "La identificación solo debe contener números")]
        public required string Identificacion { get; set; }

        [Required(ErrorMessage = "El nombre completo es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder 100 caracteres")]
        [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑ\s]+$", ErrorMessage = "El nombre solo puede contener letras y espacios")]
        public required string Nombre { get; set; }

        [Required(ErrorMessage = "El email es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [StringLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "El teléfono es obligatorio")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        [StringLength(20, ErrorMessage = "El teléfono no puede exceder 20 caracteres")]
        public required string Telefono { get; set; }

        [StringLength(200, ErrorMessage = "La dirección no puede exceder 200 caracteres")]
        public required string Direccion { get; set; }
    }
}
