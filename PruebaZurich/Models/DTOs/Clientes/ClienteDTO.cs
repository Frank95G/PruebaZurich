using PruebaZurich.Models.DTOs.Polizas;

namespace PruebaZurich.Models.DTOs.Clientes
{
    public class ClienteDTO
    {
        public int ClienteId { get; set; }
        public string Identificacion { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public DateTime FechaRegistro { get; set; }
    }
}