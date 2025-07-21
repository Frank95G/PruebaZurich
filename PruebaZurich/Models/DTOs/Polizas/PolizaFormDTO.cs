using System.ComponentModel.DataAnnotations;

namespace PruebaZurich.Models.DTOs.Polizas
{
    public class PolizaFormDTO
    {
        [Required(ErrorMessage = "El ID del cliente es obligatorio")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del cliente no es válido")]
        public int ClienteId { get; set; }

        [Required(ErrorMessage = "El tipo de póliza es obligatorio")]
        [Range(1, 4, ErrorMessage = "El tipo de póliza no es válido")]
        public int TipoPolizaId { get; set; }

        [Required(ErrorMessage = "La fecha de inicio es obligatoria")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "La fecha de expiración es obligatoria")]
        public DateTime FechaExpiracion { get; set; }

        [Required(ErrorMessage = "El monto asegurado es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor a cero")]
        public decimal MontoAsegurado { get; set; }
        public string? Estado { get; set; }
    }
}
