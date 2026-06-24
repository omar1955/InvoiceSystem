using InvoiceSystem.API.Models;



namespace InvoiceSystem.API.Services
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
    }
}