using Domain.Entities;
using Domain.Interfaces;
using Application.Commands;
using MediatR;
using Application.Dtos;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Application.CommandHandlers
{
    public class CreateCourseHandler : IRequestHandler<CreateCourseCommand, CourseDto?>
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IUserRepository _userRepository;
        private readonly IPdfToMarkdownService _pdfToMarkdownService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        
        public CreateCourseHandler(ICourseRepository courseRepository, IUserRepository userRepository, IPdfToMarkdownService pdfToMarkdownService, IHttpContextAccessor httpContextAccessor)
        {
            _courseRepository = courseRepository;
            _userRepository = userRepository;
            _pdfToMarkdownService = pdfToMarkdownService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<CourseDto?> Handle(CreateCourseCommand command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Title))
                throw new InvalidOperationException("Title can't be null.");
            if (command.FileContent == null)
                throw new InvalidOperationException("File Content can't be null.");

            
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrWhiteSpace(userId))
                throw new UnauthorizedAccessException("User is not authenticated.");
            
            var user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var fileContent = await _pdfToMarkdownService.ConvertPdfToMarkdownAsync(command.FileContent);

            var course = new Course
            {
                Id = Guid.NewGuid().ToString(),
                Title = command.Title,
                Content = fileContent,
                CreatedAt = command.CreatedAt,
                FileContent = command.FileContent,
                UserId = userId,
                User = user,
                Status = "In Progress",
            };

            var createdCourse = await _courseRepository.CreateCourse(course);

            var courseResponse = new CourseDto
            {
                Id = createdCourse.Id,
                Status = createdCourse.Status,
                Title = createdCourse.Title,
                Subject = createdCourse.Subject,
                Content = createdCourse.Content,
                FileContent = createdCourse.FileContent,
                CreatedAt = createdCourse.CreatedAt
            };

            return courseResponse;
        }
    }
}
