namespace InvoiceSystem.API.Models
{
    public class InvoiceItem
    {
        public int Id { get; set; }

        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; } = null!;

        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        public int Quantity { get; set; }

        public decimal Price { get; set; }

        public decimal DiscountPercentage { get; set; }

        public decimal TaxPercentage { get; set; }

        public decimal Total { get; set; }
    }
}