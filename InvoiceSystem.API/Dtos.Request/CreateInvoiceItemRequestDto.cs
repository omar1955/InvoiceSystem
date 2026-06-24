namespace InvoiceSystem.API.Dtos.Request
{
    public class CreateInvoiceItemRequestDto
    {
        public int ProductId { get; set; }

        public int Quantity { get; set; }

      

        public decimal DiscountPercentage { get; set; }

        public decimal TaxPercentage { get; set; }
    }
}