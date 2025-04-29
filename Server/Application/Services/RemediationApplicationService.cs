using Application.Dtos;
using Application.Interfaces;
using Domain.Interfaces;

namespace Application.Services;

public sealed class RemediationApplicationService
{
    private readonly IQuizRepository _quizzes;
    private readonly IWeakTopicDetectionService _detector;
    private readonly IRemediationService _remediator;
    private readonly IQuizGenerationService _quizGen;

    public RemediationApplicationService(
        IQuizRepository quizzes,
        IWeakTopicDetectionService detector,
        IRemediationService remediator,
        IQuizGenerationService quizGen)
        => (_quizzes, _detector, _remediator, _quizGen) =
            (quizzes, detector, remediator, quizGen);

    public async Task<(string remediationJson, QuizDto followUpQuiz)> RemediateAsync(
        string quizId, CancellationToken ct)
    {
        var quiz = await _quizzes.GetQuizById(quizId);
        var questionsDto = new List<QuestionDto>();

        foreach (var item in quiz)
        {
            var dto = new QuestionDto()
            {
                Id = item.Id,
                Answer = item.CorrectAnswers,
                Choices = item.Choices,
                Content = item.Content,
                isUserAnswerCorrectly = item.isUserAnswerCorrectly,
                Type = item.Type,
            };

            questionsDto.Add(dto);
        }

        var weak = await _detector.DetectWeakTopicsAsync(questionsDto, ct);

        var remediationJson = await _remediator.BuildRemediationAsync(weak, ct);

        var remediationCourse = new CourseDto
        {
            Id = Guid.NewGuid().ToString(),
            Status = Status.InProgress,
            Title = "Remediation for " + quiz,
            Content = remediationJson,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var followUp = await _quizGen.GenerateQuizAsync(remediationCourse, ct);
        return (remediationJson, followUpQuiz: followUp);
    }
}