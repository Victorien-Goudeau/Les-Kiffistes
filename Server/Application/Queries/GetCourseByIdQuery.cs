namespace Application.Queries
{
    using Domain.Entities;
    using MediatR;

    public class GetCourseByIdQuery : IRequest<Course?>
    {
        public required string CourseId { get; set; }
    }
}