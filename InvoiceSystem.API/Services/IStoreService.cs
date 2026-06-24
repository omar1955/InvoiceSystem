using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Services
{
    public interface IStoreService
    {
        Task<List<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
    }
}