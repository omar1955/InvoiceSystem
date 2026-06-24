using AutoMapper;
using InvoiceSystem.API.Dtos.Request;
using InvoiceSystem.API.Dtos.Response;
using InvoiceSystem.API.Helpers;
using InvoiceSystem.API.Models;
using InvoiceSystem.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InvoiceSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class InvoicesController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IMapper _mapper;

        public InvoicesController(IInvoiceService invoiceService, IMapper mapper)
        {
            _invoiceService = invoiceService;
            _mapper = mapper;
        }

        [HttpGet]
       
        public async Task<IActionResult> GetAll([FromQuery] InvoiceQueryRequestDto request)
        {
            var pagedInvoices = await _invoiceService.GetAllAsync(
                request.Search,
                request.PageNumber,
                request.PageSize
            );

            var mappedItems = _mapper.Map<List<InvoiceListResponseDto>>(pagedInvoices.Items);

            var response = new PagedResult<InvoiceListResponseDto>
            {
                Items = mappedItems,
                PageNumber = pagedInvoices.PageNumber,
                PageSize = pagedInvoices.PageSize,
                TotalCount = pagedInvoices.TotalCount
            };

            return Ok(new
            {
                success = true,
                message = "Invoices retrieved successfully.",
                data = response
            });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var invoice = await _invoiceService.GetByIdAsync(id);

            var response = _mapper.Map<InvoiceResponseDto>(invoice);

            return Ok(new
            {
                success = true,
                message = "Invoice retrieved successfully.",
                data = response
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateInvoiceRequestDto request)
        {
            var invoice = _mapper.Map<Invoice>(request);

            var createdInvoice = await _invoiceService.CreateAsync(invoice);

            var response = _mapper.Map<InvoiceResponseDto>(createdInvoice);

            return Ok(new
            {
                success = true,
                message = "Invoice created successfully.",
                data = response
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateInvoiceRequestDto request)
        {
            var invoice = _mapper.Map<Invoice>(request);

            var updatedInvoice = await _invoiceService.UpdateAsync(id, invoice);

            var response = _mapper.Map<InvoiceResponseDto>(updatedInvoice);

            return Ok(new
            {
                success = true,
                message = "Invoice updated successfully.",
                data = response
            });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _invoiceService.DeleteAsync(id);

            return Ok(new
            {
                success = true,
                message = "Invoice deleted successfully."
            });
        }
    }
}