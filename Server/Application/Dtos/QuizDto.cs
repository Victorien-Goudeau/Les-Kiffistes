namespace Application.Dtos
{
    public class QuizDto
    {
        public required string Id { get; set; }
        public required Status? Status { get; set; } // "Pending", "In Progress", "Failed" or "Succeeded"
        public required string CourseId { get; set; }
        public string? Title { get; set; }
        public List<QuestionDto> Questions { get; set; } = new();
    }
}