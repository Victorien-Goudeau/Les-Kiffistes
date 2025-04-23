using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> RegisterUserAsync(User user);
        Task<User?> GetUserByUsernameOrEmailAsync(string username);
        Task<User?> GetUserByIdAsync(string userId);
        Task<bool> IsUsernameTakenAsync(string username);
        Task<bool> IsEmailTakenAsync(string username);
    }
}