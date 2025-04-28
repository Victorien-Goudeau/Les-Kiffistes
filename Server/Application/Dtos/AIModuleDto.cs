namespace Application.Dtos
{
    public class AIModuleDto
    {
        public required string Id { get; set; }
        public required string? Title { get; set; }
        public required string? Content { get; set; }
        public required Status Status { get; set; } // "In Progress" or "Finished"
    }
}