// using Application.Dtos;
// using Application.Queries;
// using Domain.Interfaces;
// using MediatR;

// namespace Application.CommandHandlers {
//     public class GetCourseByIdQuery : IRequestHandler<GetCourseModulesQuery, List<AIModuleDto>> {
//         private readonly ICourseRepository _icourseRepository;

//         public GetCourseByIdQuery(ICourseRepository icourseRepository) {
//             _icourseRepository = icourseRepository;
//         }

//         public async Task<List<AIModuleDto>> Handle(GetCourseModulesQuery request, CancellationToken cancellationToken)
//         {
//             var aimodules = await _icourseRepository.GetModulesByCourseId(request.CourseId);

//             if (aimodules == null) {
//                 throw new KeyNotFoundException("Modules related to this course not found.");
//             }

//             return aimodules.Select(m => new AIModuleDto() {
//                 Id = m.Id,
//                 Title = m.Title,
//                 Content = m.Content,
//                 Status = m.Status,
//             }).ToList();
//         }
//     }
// }
