using Backend_Gym_Iris.DTOs.User;
using Backend_Gym_Iris.Entities;

namespace Backend_Gym_Iris.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetAllUsersWithPaymentsAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateUserAsync(User user);
        Task<User?> UpdateUserAsync(int id, User user);
        Task<bool> DeleteUserAsync(int id);
        UserResponse MapToResponse(User u);
    }
}
