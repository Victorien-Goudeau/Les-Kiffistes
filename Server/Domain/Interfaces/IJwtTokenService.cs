using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}