using Application.Dtos;
using MediatR;

namespace Application.Queries
{
    public class GetCurrentUserQuery : IRequest<UserDto>
    {
        public required string UserId { get; set; }
    }
}