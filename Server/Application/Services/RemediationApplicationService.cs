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
        string quizId, IDictionary<string,double> scores, CancellationToken ct)
    {
        var quiz = await _quizzes.GetQuizByCourseId(quizId);
        
        //map quiz to DTO
        var quizDto = new QuizDto
        {
            Id        = quiz.Id,
            CourseId  = quiz.CourseId,
            Title     = quiz.Title,
            Status    = quiz.Status,
            Questions = quiz.Questions.Select(q => new QuestionDto
            {
                Id      = q.Id,
                Content = q.Content,
                Type    = q.Type,
                Choices = q.Choices
            }).ToList()
        };

        var weak = await _detector.DetectWeakTopicsAsync(quizDto, scores, ct);
        if (!weak.Any())
            return ("{}", quizDto);

        var remediationJson = await _remediator.BuildRemediationAsync(weak, ct);

        var remediationCourse = new CourseDto
        {
            Id        = Guid.NewGuid().ToString(),
            Status    = Status.InProgress,
            Title     = "Remediation for " + quiz.Title,
            Content   = remediationJson,
            CreatedAt = DateTimeOffset.UtcNow
        };

        var followUp = await _quizGen.GenerateQuizAsync(remediationCourse, ct);
        return (remediationJson, followUpQuiz: followUp);
    }
}