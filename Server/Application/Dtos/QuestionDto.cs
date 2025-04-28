namespace Application.Dtos
{
    public class QuestionDto
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public required QuestionType Type { get; set; } // "Checkbox", "Multiple Choice" or "Open"
        public List<string> Choices { get; set; } = new();
    }
}