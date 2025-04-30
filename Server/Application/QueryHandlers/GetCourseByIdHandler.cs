using Application.Dtos;
using Application.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers {
    public class GetCourseByIdHandler : IRequestHandler<GetCourseByIdQuery, CourseDto?> {
        private readonly ICourseRepository _icourseRepository;

        public GetCourseByIdHandler(ICourseRepository icourseRepository) {
            _icourseRepository = icourseRepository;
        }

        public async Task<CourseDto?> Handle(GetCourseByIdQuery request, CancellationToken cancellationToken)
        {
            var course = await _icourseRepository.GetCourseById(request.CourseId);

            if (course == null) {
                throw new KeyNotFoundException("Modules related to this course not found.");
            }

            return new CourseDto() {
                Id= course.Id,
                Status = course.Status,
                Title = course.Title,
                Subject = course.Subject,
                Content = course.Content,
                FileContent = course.FileContent,
                CreatedAt = course.CreatedAt
            };
        }
    }
}
