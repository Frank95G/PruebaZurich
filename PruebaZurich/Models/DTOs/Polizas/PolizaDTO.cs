namespace PruebaZurich.Models.DTOs.Polizas
{
    public class PolizaDTO
    {
        public int PolizaId { get; set; }
        public string NumeroPoliza { get; set; }
        public int ClienteId { get; set; }
        public string ClienteNombre { get; set; }
        public int TipoPolizaId { get; set; }
        public string TipoPoliza { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public decimal MontoAsegurado { get; set; }
        public string Estado { get; set; }
        public DateTime FechaEmision { get; set; }
    }
}
