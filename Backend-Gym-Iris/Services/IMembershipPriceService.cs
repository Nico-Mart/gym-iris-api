using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Enums;

namespace Backend_Gym_Iris.Services
{
    public interface IMembershipPriceService
    {
        Task<IEnumerable<MembershipPrice>> GetAllPricesAsync();
        Task<MembershipPrice?> GetPriceByIdAsync(int id);
        Task<MembershipPrice?> GetPriceByTypeAsync(MembershipType type);
        Task<MembershipPrice> UpdatePriceAsync(int id, decimal newPrice);
    }
}
