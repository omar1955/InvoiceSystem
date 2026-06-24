using InvoiceSystem.API.Helpers;
using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Repositories
{
    public interface IInvoiceRepository
    {
        Task<PagedResult<Invoice>> GetAllAsync(string? search, int pageNumber, int pageSize);

        Task<Invoice?> GetByIdWithDetailsAsync(int id);

        Task<Invoice?> GetByIdForUpdateAsync(int id);

        Task<int> GetMaxIdAsync();

        Task AddAsync(Invoice invoice);

        void Delete(Invoice invoice);

        Task SaveChangesAsync();
    }
}