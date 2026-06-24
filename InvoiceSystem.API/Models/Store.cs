namespace InvoiceSystem.API.Models
{
    public class Store
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Address { get; set; }

        public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}