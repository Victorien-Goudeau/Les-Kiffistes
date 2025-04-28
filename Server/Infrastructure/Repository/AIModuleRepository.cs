using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class AIModuleRepository : IAIModuleRepository
    {
        private readonly AppDbContext _context;

        public AIModuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<AIModule>?> GetModulesByCourseId(string id) {
            return await _context.AIModules
                .Where(m => m.CourseId == id)
                .ToListAsync();
        }

        public async Task<bool> DeleteModule(AIModule aimodule) {
            _context.AIModules.Remove(aimodule);
            var affected = await _context.SaveChangesAsync();
            return affected > 0;
        }
    }
}