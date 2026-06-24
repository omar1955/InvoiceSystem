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
    public class StoresController : ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly IMapper _mapper;

        public StoresController(IStoreService storeService, IMapper mapper)
        {
            _storeService = storeService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var stores = await _storeService.GetAllAsync();

            var response = _mapper.Map<List<StoreResponseDto>>(stores);

            return Ok(new
            {
                success = true,
                message = "Stores retrieved successfully.",
                data = response
            });
        }
    }
}