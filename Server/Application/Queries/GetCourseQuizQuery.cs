using Application.Dtos;
using MediatR;

namespace Application.Queries
{
    public class GetCourseQuizQuery : IRequest<QuizDto>
    {
        public required string CourseId { get; set; }
    }
}