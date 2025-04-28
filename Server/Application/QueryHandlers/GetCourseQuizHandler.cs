using Application.Dtos;
using Application.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers {
    public class GetCourseQuizHandler : IRequestHandler<GetCourseQuizQuery, QuizDto> {
        private readonly IQuizRepository _quizRepository;

        public GetCourseQuizHandler(IQuizRepository quizRepository, ICourseRepository courseRepository) {
            _quizRepository = quizRepository;
        }

        public async Task<QuizDto> Handle(GetCourseQuizQuery query, CancellationToken cancellationToken) {

            var quiz = await _quizRepository.GetQuizByCourseId(query.CourseId);
            
            if (quiz == null) {
                throw new KeyNotFoundException("Quiz related to this course not found.");
            }

            return new QuizDto() {
                Id = quiz.Id,
                Status = quiz.Status,
                CourseId = quiz.CourseId,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuestionDto() {
                    Id = q.Id,
                    Content = q.Content,
                    Type = q.Type,
                    Choices = q.Choices
                }).ToList()
            };
        }
    }
}