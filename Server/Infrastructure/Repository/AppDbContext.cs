using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // ── USER ───────────────────────────────────────────────────────────────
            modelBuilder.Entity<User>(b =>
            {
                b.ToContainer("Users");
                b.HasKey(u => u.Id);
                b.Property(u => u.Id).ToJsonProperty("id");
                b.HasPartitionKey(u => u.UserName);

                b.HasMany(u => u.Courses)
                .WithOne(c => c.User)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // ── COURSE ─────────────────────────────────────────────────────────────
            modelBuilder.Entity<Course>(b =>
            {
                b.ToContainer("Courses");
                b.HasKey(c => c.Id);
                b.Property(c => c.Id).ToJsonProperty("id");
                b.HasPartitionKey(c => c.UserId);

                b.HasMany(c => c.Quiz)           // attention au nom de la collection
                .WithOne(q => q.Course)
                .HasForeignKey(q => q.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasMany(c => c.AIModules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // ── AIMODULE ↔ QUIZ (1-à-1) ─────────────────────────────────────────────
            // On fait dépendant Quiz sur AIModule via Quiz.AIModuleId
            modelBuilder.Entity<AIModule>(b =>
            {
                b.ToContainer("AIModules");
                b.HasKey(m => m.Id);
                b.Property(m => m.Id).ToJsonProperty("id");
                b.HasPartitionKey(m => m.CourseId);

                b.HasOne(m => m.Quiz)
                .WithOne(q => q.AIModule)
                .HasForeignKey<Quiz>(q => q.AIModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // ── QUIZ ────────────────────────────────────────────────────────────────
            modelBuilder.Entity<Quiz>(b =>
            {
                b.ToContainer("Quiz");
                b.HasKey(q => q.Id);
                b.Property(q => q.Id).ToJsonProperty("id");
                b.HasPartitionKey(q => q.CourseId);

                b.HasOne(q => q.AIModule)
                .WithOne(m => m.Quiz)
                .HasForeignKey<AIModule>(q => q.QuizId);

                b.HasMany(q => q.Questions)
                .WithOne(qt => qt.Quiz)
                .HasForeignKey(qt => qt.QuizId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            // ── QUESTION ────────────────────────────────────────────────────────────
            modelBuilder.Entity<Question>(b =>
            {
                b.ToContainer("Questions");
                b.HasKey(q => q.Id);
                b.Property(q => q.Id).ToJsonProperty("id");
                b.HasPartitionKey(q => q.QuizId);
            });
        }
    }
}
