using Application.Dtos;
using MediatR;

namespace Application.Queries
{
    public class GetUserCoursesQuery : IRequest<List<CourseDto>>
    {
        public required string UserId { get; set; }
    }
}