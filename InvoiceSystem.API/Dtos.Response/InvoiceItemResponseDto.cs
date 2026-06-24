namespace InvoiceSystem.API.Dtos.Response
{
    public class InvoiceItemResponseDto
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal TaxPercentage { get; set; }

        public decimal Total { get; set; }
    }
}