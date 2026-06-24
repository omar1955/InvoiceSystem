namespace InvoiceSystem.API.Dtos.Request
{
    public class InvoiceQueryRequestDto
    {
        public string? Search { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}