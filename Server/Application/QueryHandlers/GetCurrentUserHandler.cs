using Application.Dtos;
using Application.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers
{
    public class GetCurrentUserHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentUserHandler(IUserRepository userRepository)
        {
            _userRepository   = userRepository;
        }

        public async Task<UserDto> Handle(GetCurrentUserQuery command, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetUserByIdAsync(command.UserId);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials.");

            return new UserDto() {
                id = user.Id,
                UserName = user.UserName,
                Email=user.Email,
                Role = user.Role
            };
        }
    }
}
