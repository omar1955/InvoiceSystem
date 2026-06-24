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
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productService.GetAllAsync();

            var response = _mapper.Map<List<ProductResponseDto>>(products);

            return Ok(new
            {
                success = true,
                message = "Products retrieved successfully.",
                data = response
            });
        }
    }
}