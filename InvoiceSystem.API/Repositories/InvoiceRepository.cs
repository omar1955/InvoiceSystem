
using global::InvoiceSystem.API.Data;
using global::InvoiceSystem.API.Models;
using global::InvoiceSystem.API.Repositories;
using InvoiceSystem.API.Helpers;
using InvoiceSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.API.Repositories
{
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResult<Invoice>> GetAllAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Invoices
                .Include(i => i.Store)
                .Include(i => i.Customer)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(i =>
                    i.Serial.Contains(search) ||
                    i.Customer.Name.Contains(search) ||
                    i.Store.Name.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var invoices = await query
                .OrderByDescending(i => i.CreatedAt)
                .ThenByDescending(i => i.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<Invoice>
            {
                Items = invoices,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<Invoice?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Store)
                .Include(i => i.Customer)
                .Include(i => i.Items)
                    .ThenInclude(item => item.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Invoice?> GetByIdForUpdateAsync(int id)
        {
            return await _context.Invoices
                .Include(i => i.Items)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<int> GetMaxIdAsync()
        {
            if (!await _context.Invoices.AnyAsync())
                return 0;

            return await _context.Invoices.MaxAsync(i => i.Id);
        }

        public async Task AddAsync(Invoice invoice)
        {
            await _context.Invoices.AddAsync(invoice);
        }

        public void Delete(Invoice invoice)
        {
            _context.Invoices.Remove(invoice);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}