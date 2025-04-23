using MediatR;

namespace Application.Commands
{
    public class RegisterUserCommand : IRequest<string>
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}
