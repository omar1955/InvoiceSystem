using InvoiceSystem.API.Helpers;
using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Services
{
    public interface IInvoiceService
    {
        Task<PagedResult<Invoice>> GetAllAsync(string? search, int pageNumber, int pageSize);

        Task<Invoice> GetByIdAsync(int id);
        Task<Invoice> CreateAsync(Invoice invoice);
        Task<Invoice> UpdateAsync(int id, Invoice invoice);
        Task DeleteAsync(int id);
    }
}