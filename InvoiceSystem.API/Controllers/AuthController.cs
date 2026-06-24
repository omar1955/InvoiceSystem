using AutoMapper;
using InvoiceSystem.API.Dtos.Request;
using InvoiceSystem.API.Dtos.Response;
using InvoiceSystem.API.Models;
using InvoiceSystem.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IMapper _mapper;

        public AuthController(IAuthService authService, IMapper mapper)
        {
            _authService = authService;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto request)
        {
            var user = _mapper.Map<ApplicationUser>(request);

            var createdUser = await _authService.RegisterAsync(user, request.Password);

            var response = _mapper.Map<RegisterResponseDto>(createdUser);

            return Ok(new
            {
                success = true,
                message = "User registered successfully. Please login to continue.",
                data = response
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto request)
        {
            var result = await _authService.LoginAsync(request.Email, request.Password);

            var response = _mapper.Map<LoginResponseDto>(result.User);
            response.Token = result.Token;

            return Ok(new
            {
                success = true,
                message = "Login successfully.",
                data = response
            });
        }
    }
}