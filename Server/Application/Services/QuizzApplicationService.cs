using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public sealed class QuizApplicationService
{
    private readonly ICourseRepository _courses;
    private readonly IQuizRepository _quizzes;   // optionnel si vous traitez le quiz via Course
    private readonly IQuizGenerationService _ai;

    public QuizApplicationService(
        ICourseRepository courses,
        IQuizRepository quizzes,
        IQuizGenerationService ai) =>
        (_courses, _quizzes, _ai) = (courses, quizzes, ai);

    public async Task<Quiz> CreateQuizForCourseAsync(string courseId, CancellationToken ct)
    {
        // 1) charger le cours avec ses quiz pour vérifier les doublons
        var course = await _courses.GetCourseById(courseId);

        if (course == null)
            throw new KeyNotFoundException($"Course with ID {courseId} not found.");

        // 3) Envoyer seulement le contenu nécessaire à l'agent
        var quizDto = await _ai.GenerateQuizAsync(DtoMapper.MapToDto(course), ct);

        // 4) Mapper en entité et rattacher à l'aggregate
        var quiz = new Quiz
        {
            Id = quizDto.Id,
            CourseId = course.Id,
            Title = quizDto.Title,
            Status = Status.NotStarted,
            GeneratedAt = DateTimeOffset.UtcNow,
            Questions = quizDto.Questions.Select(q => new Question
            {
                Id = Guid.NewGuid().ToString(),
                QuizId = quizDto.Id,
                
                Content = q.Content,
                Type = q.Type,
                Choices = q.Choices,
                CorrectAnswers = q.Answer,
            }).ToList()
        };

        course.Quiz.Add(quiz);          // cohérence de l'aggregate

        // 5) Sauvegarder via le dépôt de Course (cascading save)
        await _courses.UpdateCourse(course);

        return quiz;
    }

    public async Task<double> SubmitQuizAsync(SubmitQuizDto submission, CancellationToken ct)
    {
        var quizLength = submission.Questions.Count;
        //Récupérer le résultat du quiz par rapport au nombre de true par rapport à le nombre de questions
        var correctAnswers = submission.Questions.Count(q => q.IsUserAnswerCorrectly);
        var score = (double)correctAnswers / quizLength * 100;
        foreach (var question in submission.Questions)
        {

            await _quizzes.UpdateQuestion(question.Id, question.IsUserAnswerCorrectly);
        }

        return score;
    }

    public static class DtoMapper
    {
        public static CourseDto MapToDto(Course c) => new()
        {
            Id = c.Id,
            Title = c.Title,
            Subject = c.Subject,
            Content = c.Content,
            CreatedAt = c.CreatedAt,
            Status = Status.InProgress
        };
    }
}