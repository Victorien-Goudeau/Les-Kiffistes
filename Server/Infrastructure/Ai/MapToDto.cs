using Application.Dtos;
using Domain.Entities;

namespace Infrastructure.Ai;

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
