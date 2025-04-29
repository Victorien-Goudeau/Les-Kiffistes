using System.Text.Json.Serialization;

namespace Domain.Entities
{
    public class Question
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        public required string QuizId { get; set; }
        public required string Content { get; set; }
        public DateTimeOffset GeneratedAt { get; set; }
        public required QuestionType Type { get; set; } // "Checkbox", "Multiple Choice" or "Open"
        public required string CorrectAnswers { get; set; }
        public required string Choices { get; set; }
        public bool? isUserAnswerCorrectly { get; set; } // true or false if the user answered correctly
    }
}