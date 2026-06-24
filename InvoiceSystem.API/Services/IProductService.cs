using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
    }
}