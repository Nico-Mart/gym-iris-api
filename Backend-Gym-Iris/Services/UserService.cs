using Backend_Gym_Iris.DTOs.User;
using Backend_Gym_Iris.Entities;
using Backend_Gym_Iris.Repositories;

namespace Backend_Gym_Iris.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }
        public async Task<IEnumerable<User>> GetAllUsersWithPaymentsAsync()
        {
            return await _userRepository.GetAllWithPaymentsAsync();
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.GetByEmailAsync(email);
        }

        public async Task<User> CreateUserAsync(User user)
        {
            
            user.Email = user.Email.ToLowerInvariant().Trim();

            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null)
            {
                throw new InvalidOperationException($"A user with email '{user.Email}' already exists.");
            }

            return await _userRepository.CreateAsync(user);
        }

        public async Task<User?> UpdateUserAsync(int id, User user)
        {
            
            user.Email = user.Email.ToLowerInvariant().Trim();

            
            var existingUser = await _userRepository.GetByEmailAsync(user.Email);
            if (existingUser != null && existingUser.Id != id)
            {
                throw new InvalidOperationException($"A user with email '{user.Email}' already exists.");
            }
            var exists = await _userRepository.ExistsAsync(id);
            if (!exists)
                return null;

            user.Id = id;

            return await _userRepository.UpdateAsync(id, user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userRepository.DeleteAsync(id);
        }
        public UserResponse MapToResponse(User u)
        {
            var payments = u.Payments.Select(p =>
            {
                var monthKey = $"{p.Year}-{p.Month:D2}";

                return new PaymentInfo(
                    p.Id, 
                    monthKey, 
                    string.Empty,
                    p.Amount,       
                    p.Amount,     
                    p.PaymentDate.ToString("yyyy-MM-dd")
                );
            }).ToList();

            return new UserResponse(
                u.Id, u.Name, u.Email, u.Telephone,
                u.JoinDate, u.Status.ToString(), u.Activity, payments
            );
        }

    }
}
