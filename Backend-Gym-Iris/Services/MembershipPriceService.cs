using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Enums;
using Backend_Gym_Iris.Repositories;

namespace Backend_Gym_Iris.Services
{
    public class MembershipPriceService : IMembershipPriceService
    {
        private readonly IMembershipPriceRepository _priceRepository;

        public MembershipPriceService(IMembershipPriceRepository priceRepository)
        {
            _priceRepository = priceRepository;
        }

        public async Task<IEnumerable<MembershipPrice>> GetAllPricesAsync()
        {
            return await _priceRepository.GetAllAsync();
        }

        public async Task<MembershipPrice?> GetPriceByIdAsync(int id)
        {
            return await _priceRepository.GetByIdAsync(id);
        }

        public async Task<MembershipPrice?> GetPriceByTypeAsync(MembershipType type)
        {
            return await _priceRepository.GetByTypeAsync(type);
        }

        public async Task<MembershipPrice> UpdatePriceAsync(int id, decimal newPrice)
        {
            if (newPrice <= 0)
                throw new ArgumentException("El precio debe ser mayor a 0");

            var existingPrice = await _priceRepository.GetByIdAsync(id);
            if (existingPrice == null)
                throw new ArgumentException($"Precio con ID {id} no encontrado");

            existingPrice.Price = newPrice;
            existingPrice.LastUpdated = DateTime.UtcNow;

            var updated = await _priceRepository.UpdateAsync(existingPrice);
            return updated!;
        }
    }
}
