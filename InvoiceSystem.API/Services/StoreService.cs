using InvoiceSystem.API.Models;
using InvoiceSystem.API.Repositories;

namespace InvoiceSystem.API.Services
{
    public class StoreService : IStoreService
    {
        private readonly IStoreRepository _storeRepository;

        public StoreService(IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository;
        }

        public async Task<List<Store>> GetAllAsync()
        {
            return await _storeRepository.GetAllAsync();
        }

        public async Task<Store?> GetByIdAsync(int id)
        {
            return await _storeRepository.GetByIdAsync(id);
        }
    }
}