using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IQuizRepository
    {
        Task<Quiz> CreateQuiz(Quiz quiz);
        Task<Quiz?> GetQuizByCourseId(string id);
        Task<bool> DeleteQuiz(Quiz quiz);
        Task<List<Question>?> GetQuizById(string id);
    }
}