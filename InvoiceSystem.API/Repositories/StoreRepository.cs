using InvoiceSystem.API.Data;
using InvoiceSystem.API.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceSystem.API.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly AppDbContext _context;

        public StoreRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _context.Stores
                .AsNoTracking()
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            return await _context.Stores
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}