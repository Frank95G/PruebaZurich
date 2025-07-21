namespace PruebaZurich.Models.DTOs.Shared
{
    public class PagedResultDTO<T>
    {
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);
        public List<T> Items { get; set; } = new List<T>();
    }
}
