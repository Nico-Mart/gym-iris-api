namespace Backend_Gym_Iris.Services
{
    public interface IAdminService
    {
        Task<bool> ValidatePasswordAsync(string password);
    }
}
