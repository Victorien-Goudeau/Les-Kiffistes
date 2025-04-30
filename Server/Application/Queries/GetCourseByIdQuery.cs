namespace Application.Queries
{
    using Application.Dtos;
    using MediatR;

    public class GetCourseByIdQuery : IRequest<CourseDto?>
    {
        public required string CourseId { get; set; }
    }
}