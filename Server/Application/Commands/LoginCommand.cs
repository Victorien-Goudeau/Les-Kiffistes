using MediatR;
namespace Application.Commands
{
    public class LoginCommand : IRequest<string>
    {
        public required string Login { get; set; } // email or userName
        public required string Password { get; set; }
    }
}