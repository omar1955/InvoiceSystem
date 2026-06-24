using Microsoft.AspNetCore.Identity;

namespace InvoiceSystem.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}