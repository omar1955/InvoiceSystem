namespace InvoiceSystem.API.Dtos.Response
{
    public class InvoiceListResponseDto
    {
        public int Id { get; set; }

        public string Serial { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string StoreName { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }
    }
}