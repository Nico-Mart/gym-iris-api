using Backend_Gym_Iris.Data;
using Microsoft.EntityFrameworkCore;

namespace Backend_Gym_Iris.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;

        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> ValidatePasswordAsync(string password)
        {
            var admin = await _context.Admins.FirstOrDefaultAsync();

            if (admin == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, admin.PasswordHash);
        }
    }
}
