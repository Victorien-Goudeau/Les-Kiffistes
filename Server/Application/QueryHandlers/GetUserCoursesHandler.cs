using Application.Dtos;
using Application.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers
{
    public class GetUserCoursesHandler : IRequestHandler<GetUserCoursesQuery, List<CourseDto>>
    {
        private readonly ICourseRepository _CourseRepository;

        public GetUserCoursesHandler(ICourseRepository courseRepository)
        {
            _CourseRepository   = courseRepository;
        }

        public async Task<List<CourseDto>> Handle(GetUserCoursesQuery command, CancellationToken cancellationToken)
        {
            var courses = await _CourseRepository.GetCoursesByUserId(command.UserId);
            
            return courses.Select(x => new CourseDto() {
                Id = x.Id,
                Status = x.Status,
                Title = x.Title,
                Subject = x.Subject,
                Content = x.Content,
                FileUrl = x.FileUrl,
                CreatedAt = x.CreatedAt,
            }).ToList();
        }
    }
}