using Application.Dtos;

namespace Application.Interfaces;

public interface IWeakTopicDetectionService
{
    Task<IReadOnlyList<string>> DetectWeakTopicsAsync(
        QuizDto quiz, IDictionary<string,double> scores, CancellationToken ct);
}