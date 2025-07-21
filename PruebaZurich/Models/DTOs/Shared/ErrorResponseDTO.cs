namespace PruebaZurich.Models.DTOs.Shared
{
    public class ErrorResponseDTO
    {
        public string Type { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public Dictionary<string, string[]> Errors { get; set; }
    }
}
