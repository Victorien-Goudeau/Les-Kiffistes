using Application.Commands;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public LoginHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository   = userRepository;
            _jwtTokenService  = jwtTokenService;
        }

        public async Task<string> Handle(LoginCommand command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByUsernameOrEmailAsync(command.Login);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(command.Password, user.Password))
                throw new UnauthorizedAccessException("Invalid credentials.");

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
