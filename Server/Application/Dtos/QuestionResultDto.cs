namespace Application.Dtos;

public class QuestionResultDto
{
    public string Id { get; set; } = null!;
    public bool IsUserAnswerCorrectly { get; set; }
}