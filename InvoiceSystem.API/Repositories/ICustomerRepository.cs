using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);

        Task AddAsync(Customer customer);
        Task SaveChangesAsync();
    }
}