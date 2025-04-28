// Infrastructure/Repositories/CourseRepository.cs
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class CourseRepository : ICourseRepository
    {
        private readonly AppDbContext _context;

        public CourseRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Course> CreateCourse(Course course)
        {
            _context.Courses.Add(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<Course> UpdateCourse(Course course)
        {
            _context.Courses.Update(course);
            await _context.SaveChangesAsync();
            return course;
        }

        public async Task<bool> DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }

        public async Task<Course?> GetCourseById(string id)
        {
            return await _context.Courses
                                 .Where(c => c.Id == id)
                                 .FirstOrDefaultAsync();
        }

        public async Task<List<Course>> GetCoursesByUserId(string userId)
        {
            return await _context.Courses
                                 .Where(c => c.UserId == userId)
                                 .ToListAsync();
        }
    }
}
