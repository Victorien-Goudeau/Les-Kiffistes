namespace Infrastructure.Repository
{
    using Domain.Entities;
    using Domain.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class QuizRepository : IQuizRepository
    {
        private readonly AppDbContext _context;

        public QuizRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Quiz> CreateQuiz(Quiz quiz)
        {
            _context.Quizzes.Add(quiz);
            await _context.SaveChangesAsync();
            return quiz;
        }

        public async Task<Quiz?> GetQuizByCourseId(string id)
        {
            return await _context.Quizzes
                .Include(q => q.Questions)
                .FirstOrDefaultAsync(q => q.CourseId == id);
        }

        public async Task<List<Question>?> GetQuizById(string id)
        {
            return await _context.Questions.Where(p => p.QuizId == id).ToListAsync();
        }

        public async Task<bool> DeleteQuiz(Quiz quiz)
        {
            _context.Quizzes.Remove(quiz);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }
    }
}