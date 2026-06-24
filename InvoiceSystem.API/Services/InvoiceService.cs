using InvoiceSystem.API.Helpers;
using InvoiceSystem.API.Hubs;
using InvoiceSystem.API.Models;
using InvoiceSystem.API.Repositories;
using Microsoft.AspNetCore.SignalR;

namespace InvoiceSystem.API.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IStoreRepository _storeRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IHubContext<InvoiceHub> _hubContext;
        public InvoiceService(
     IInvoiceRepository invoiceRepository,
     IStoreRepository storeRepository,
     ICustomerRepository customerRepository,
     IProductRepository productRepository,
     IHubContext<InvoiceHub> hubContext)
        {
            _invoiceRepository = invoiceRepository;
            _storeRepository = storeRepository;
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _hubContext = hubContext;
        }

        public async Task<PagedResult<Invoice>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 10;

            if (pageSize > 100)
                pageSize = 100;

            return await _invoiceRepository.GetAllAsync(search, pageNumber, pageSize);
        }

        public async Task<Invoice> GetByIdAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdWithDetailsAsync(id);

            if (invoice == null)
                throw new Exception("Invoice not found.");

            return invoice;
        }

        public async Task<Invoice> CreateAsync(Invoice invoice)
        {
            await ValidateInvoiceAsync(invoice);

            invoice.Serial = await GenerateSerialAsync();
            invoice.InvoiceDate = DateTime.UtcNow;
            invoice.CreatedAt = DateTime.UtcNow;

            CalculateInvoice(invoice);

            await _invoiceRepository.AddAsync(invoice);
            await _invoiceRepository.SaveChangesAsync();

            var createdInvoice = await _invoiceRepository.GetByIdWithDetailsAsync(invoice.Id);

            if (createdInvoice == null)
                throw new Exception("Invoice created but could not be loaded.");

            await _hubContext.Clients.All.SendAsync("InvoicesUpdated", new
            {
                action = "created",
                invoiceId = createdInvoice.Id
            });

            return createdInvoice;
        }

        public async Task<Invoice> UpdateAsync(int id, Invoice invoice)
        {
            var existingInvoice = await _invoiceRepository.GetByIdForUpdateAsync(id);

            if (existingInvoice == null)
                throw new Exception("Invoice not found.");

            await ValidateInvoiceAsync(invoice);

            existingInvoice.StoreId = invoice.StoreId;
            existingInvoice.CustomerId = invoice.CustomerId;
            existingInvoice.Note = invoice.Note;

            existingInvoice.Items.Clear();

            foreach (var item in invoice.Items)
            {
                existingInvoice.Items.Add(new InvoiceItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    DiscountPercentage = item.DiscountPercentage,
                    TaxPercentage = item.TaxPercentage
                });
            }

            CalculateInvoice(existingInvoice);

            await _invoiceRepository.SaveChangesAsync();

            var updatedInvoice = await _invoiceRepository.GetByIdWithDetailsAsync(id);

            if (updatedInvoice == null)
                throw new Exception("Invoice updated but could not be loaded.");
            await _hubContext.Clients.All.SendAsync("InvoicesUpdated", new
            {
                action = "updated",
                invoiceId = updatedInvoice.Id
            });

            return updatedInvoice;
        }

        public async Task DeleteAsync(int id)
        {
            var invoice = await _invoiceRepository.GetByIdForUpdateAsync(id);

            if (invoice == null)
                throw new Exception("Invoice not found.");

            _invoiceRepository.Delete(invoice);
            await _invoiceRepository.SaveChangesAsync();
            await _hubContext.Clients.All.SendAsync("InvoicesUpdated", new
            {
                action = "deleted",
                invoiceId = id
            });
        }

        private async Task ValidateInvoiceAsync(Invoice invoice)
        {
            var store = await _storeRepository.GetByIdAsync(invoice.StoreId);

            if (store == null)
                throw new Exception("Store not found.");

            var customer = await _customerRepository.GetByIdAsync(invoice.CustomerId);

            if (customer == null)
                throw new Exception("Customer not found.");

            if (invoice.Items == null || !invoice.Items.Any())
                throw new Exception("Invoice must contain at least one item.");

            foreach (var item in invoice.Items)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);

                if (product == null)
                    throw new Exception($"Product with id {item.ProductId} not found.");

               
                    item.Price = product.Price;
            }
        }

        private void CalculateInvoice(Invoice invoice)
        {
            foreach (var item in invoice.Items)
            {
                var subtotal = item.Quantity * item.Price;

                var discountValue = subtotal * item.DiscountPercentage / 100;

                var afterDiscount = subtotal - discountValue;

                var taxValue = afterDiscount * item.TaxPercentage / 100;

                item.Total = afterDiscount + taxValue;
            }

            invoice.TotalPrice = invoice.Items.Sum(i => i.Total);
        }

        private async Task<string> GenerateSerialAsync()
        {
            var maxId = await _invoiceRepository.GetMaxIdAsync();

            var nextNumber = maxId + 1;

            return $"INV-{DateTime.UtcNow.Year}-{nextNumber.ToString("D4")}";
        }
    }
}