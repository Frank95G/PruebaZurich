using System.ComponentModel.DataAnnotations;

namespace PruebaZurich.Models.DTOs.Polizas
{
    public class CancelarPolizaDTO
    {
        [Required(ErrorMessage = "El motivo de cancelación es obligatorio")]
        [StringLength(255, ErrorMessage = "El motivo no puede exceder 255 caracteres")]
        public required string Motivo { get; set; }
    }
}
