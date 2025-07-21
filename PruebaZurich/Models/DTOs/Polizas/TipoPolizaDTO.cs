namespace PruebaZurich.Models.DTOs.Polizas
{
    public class TipoPolizaDTO
    {
        public int TipoPolizaId { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Descripcion { get; set; }
    }
}
