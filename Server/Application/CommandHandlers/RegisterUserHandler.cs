using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using MediatR;

namespace Application.CommandHandlers
{
    public class RegisterUserHandler : IRequestHandler<RegisterUserCommand, string>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtTokenService _jwtTokenService;

        public RegisterUserHandler(IUserRepository userRepository, IJwtTokenService jwtTokenService)
        {
            _userRepository = userRepository;
            _jwtTokenService = jwtTokenService;
        }

        public async Task<string> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            if (await _userRepository.IsUsernameTakenAsync(command.UserName))
                throw new InvalidOperationException("Username already taken.");
            
            if (await _userRepository.IsEmailTakenAsync(command.Email))
                throw new InvalidOperationException("Email already taken.");

            // Hash the password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(command.Password);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = command.UserName,
                Email = command.Email,
                Password = passwordHash,
                Role = command.Role
            };

            user = await _userRepository.RegisterUserAsync(user);

            return _jwtTokenService.GenerateToken(user);
        }
    }
}
