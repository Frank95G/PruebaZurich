namespace PruebaZurich.Models.DTOs.Polizas
{
    public class PolizaFilterDTO
    {
        public string? NumeroPoliza { get; set; }
        public int? ClienteId { get; set; }
        public int? TipoPolizaId { get; set; }
        public string? Estado { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
