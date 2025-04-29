namespace Application.Dtos;

public class SubmitQuizDto
{
    public string QuizId { get; set; } = null!;
    public List<QuestionResultDto> Questions { get; set; } = new();
}