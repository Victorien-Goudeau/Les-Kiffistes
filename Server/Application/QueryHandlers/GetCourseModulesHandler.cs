using Application.Dtos;
using Application.Queries;
using Domain.Interfaces;
using MediatR;

namespace Application.CommandHandlers {
    public class GetCourseModulesHandler : IRequestHandler<GetCourseModulesQuery, List<AIModuleDto>> {
        private readonly IAIModuleRepository _aimoduleRepository;

        public GetCourseModulesHandler(IAIModuleRepository aimoduleRepository) {
            _aimoduleRepository = aimoduleRepository;
        }

        public async Task<List<AIModuleDto>> Handle(GetCourseModulesQuery request, CancellationToken cancellationToken)
        {
            var aimodules = await _aimoduleRepository.GetModulesByCourseId(request.CourseId);
            
            if (aimodules == null) {
                throw new KeyNotFoundException("Modules related to this course not found.");
            }

            return aimodules.Select(m => new AIModuleDto() {
                Id = m.Id,
                Title = m.Title,
                Content = m.Content,
                Status = m.Status,
            }).ToList();
        }
    }
}