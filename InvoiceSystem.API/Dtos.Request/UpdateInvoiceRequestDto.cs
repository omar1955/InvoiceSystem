namespace InvoiceSystem.API.Dtos.Request
{
    public class UpdateInvoiceRequestDto
    {
        public int StoreId { get; set; }

        public int CustomerId { get; set; }

        public string? Note { get; set; }

        public List<CreateInvoiceItemRequestDto> Items { get; set; } = new();
    }
}