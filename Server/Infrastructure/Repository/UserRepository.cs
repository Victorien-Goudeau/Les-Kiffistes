using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User> RegisterUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string login)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == login || u.Email == login );
        }
        public async Task<User?> GetUserByIdAsync(string userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.id == userId);
        }
        public async Task<bool> IsUsernameTakenAsync(string username)
        {
            var user = await _context.Users
                .Where(u => u.UserName == username).FirstOrDefaultAsync();
            return user != null;
        }
        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var user = await _context.Users
                .Where(u => u.Email == email).FirstOrDefaultAsync();
            return user != null;
        }
    }
}
