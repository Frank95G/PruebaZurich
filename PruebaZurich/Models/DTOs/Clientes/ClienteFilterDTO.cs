namespace PruebaZurich.Models.DTOs.Clientes
{
    public class ClienteFilterDTO
    {
        //public string SearchTerm { get; set; }
        public string? Identificacion { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
