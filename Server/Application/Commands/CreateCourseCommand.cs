using Application.Dtos;
using MediatR;

namespace Application.Commands
{
    public class CreateCourseCommand : IRequest<CourseDto?>
    {
        public required string Title { get; set; }
        public required string FileContent { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
    }
}
