using InvoiceSystem.API.Models;

namespace InvoiceSystem.API.Services
{
    public interface IAuthService
    {
        Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password);

        Task<(ApplicationUser User, string Token)> LoginAsync(string email, string password);
    }
}