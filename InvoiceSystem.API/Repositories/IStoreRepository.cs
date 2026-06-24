using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Repositories
{
    public interface IStoreRepository
    {
        Task<List<Store>> GetAllAsync();
        Task<Store?> GetByIdAsync(int id);
    }
}