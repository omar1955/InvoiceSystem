namespace InvoiceSystem.API.Models
{
    public class Invoice
    {
        public int Id { get; set; }

        public string Serial { get; set; } = string.Empty;

        public DateTime InvoiceDate { get; set; }

        public string? Note { get; set; }

        public int StoreId { get; set; }
        public Store Store { get; set; } = null!;

        public int CustomerId { get; set; }
        public Customer Customer { get; set; } = null!;

        public decimal TotalPrice { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
    }
}