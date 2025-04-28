using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAIModuleRepository
    {
        Task<List<AIModule>?> GetModulesByCourseId(string id);
        Task<bool> DeleteModule(AIModule aimodule);
    }
}