using Application.Dtos;
using MediatR;

namespace Application.Queries
{
    public class GetCourseModulesQuery : IRequest<List<AIModuleDto>>
    {
        public required string CourseId { get; set; }
    }
}