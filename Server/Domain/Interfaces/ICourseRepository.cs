using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICourseRepository
    {
        Task<Course> CreateCourse(Course course);
        Task<Course> UpdateCourse(Course course);
        Task<bool> DeleteCourse(Course course);
        Task<Course?> GetCourseById(string id);
        Task<List<Course>> GetCoursesByUserId(string userId);
    }
}