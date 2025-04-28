using Application.Dtos;

namespace Application.Interfaces;

public interface IQuizGenerationService
{
    Task<QuizDto> GenerateQuizAsync(CourseDto course, CancellationToken ct);
}