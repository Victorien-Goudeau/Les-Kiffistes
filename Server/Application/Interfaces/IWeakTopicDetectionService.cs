using Application.Dtos;

namespace Application.Interfaces;

public interface IWeakTopicDetectionService
{
    Task<IReadOnlyList<string>> DetectWeakTopicsAsync(
        List<QuestionDto> questions, CancellationToken ct);
}