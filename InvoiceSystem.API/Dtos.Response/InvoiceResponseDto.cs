using InvoiceSystem.API.Dtos.Response;

namespace InvoiceSystem.API.Dtos.Response
{

    public class InvoiceResponseDto
    {
        public int Id { get; set; }

        public string Serial { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public string? Note { get; set; }

        public int StoreId { get; set; }

        public string StoreName { get; set; } = string.Empty;

        public int CustomerId { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public decimal TotalPrice { get; set; }

        public List<InvoiceItemResponseDto> Items { get; set; } = new();
    }

}