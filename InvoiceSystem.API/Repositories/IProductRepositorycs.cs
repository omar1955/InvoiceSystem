using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
    }
}