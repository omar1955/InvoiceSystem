using InvoiceSystem.API.Models;
using InvoiceSystem.API.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace InvoiceSystem.API.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly ICustomerRepository _customerRepository;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ICustomerRepository customerRepository
            )
        {
            _userManager = userManager;
            _configuration = configuration;
            _customerRepository = customerRepository;
        }

        public async Task<ApplicationUser> RegisterAsync(ApplicationUser user, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email!);

            if (existingUser != null)
                throw new Exception("Email already exists.");

            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception(errors);
            }

            var customer = new Customer
            {
                Name = user.FullName,
                Email = user.Email,
                ApplicationUserId = user.Id
            };

            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveChangesAsync();

            return user;
        }

        public async Task<(ApplicationUser User, string Token)> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
                throw new Exception("Invalid email or password.");

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, password);

            if (!isPasswordValid)
                throw new Exception("Invalid email or password.");

            var token = GenerateJwtToken(user);

            return (user, token);
        }

        private string GenerateJwtToken(ApplicationUser user)
        {
            var jwtSettings = _configuration.GetSection("Jwt");

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email!)
            };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings["Key"]!)
            );

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var expires = DateTime.UtcNow.AddMinutes(
                Convert.ToDouble(jwtSettings["DurationInMinutes"])
            );

            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}