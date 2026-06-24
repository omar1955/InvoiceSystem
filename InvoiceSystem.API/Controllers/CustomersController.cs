using AutoMapper;
using InvoiceSystem.API.Dtos.Response;
using InvoiceSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly IMapper _mapper;

        public CustomersController(ICustomerService customerService, IMapper mapper)
        {
            _customerService = customerService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var customers = await _customerService.GetAllAsync();

            var response = _mapper.Map<List<CustomerResponseDto>>(customers);

            return Ok(new
            {
                success = true,
                message = "Customers retrieved successfully.",
                data = response
            });
        }
    }
}