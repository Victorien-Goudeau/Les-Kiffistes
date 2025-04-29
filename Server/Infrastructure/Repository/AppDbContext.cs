using Microsoft.EntityFrameworkCore;
using Domain.Entities;

namespace Infrastructure.Repository
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Quiz> Quizzes { get; set; }
        public DbSet<AIModule> AIModules { get; set; }
        public DbSet<Question> Questions { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<Course>(b =>
            {
                b.ToContainer("Courses");
                b.HasKey(c => c.Id);
                b.Property(c => c.Id).ToJsonProperty("id");
                b.HasPartitionKey(c => c.UserId);

                b.HasMany(c => c.AIModules)
                .WithOne(m => m.Course)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);

                b.Property(b => b.Status).HasConversion(
                    s => s.ToString(),
                    s => (Status)Enum.Parse(typeof(Status), s));
            });

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

                b.Property(b => b.Status).HasConversion(
                    s => s.ToString(),
                    s => (Status)Enum.Parse(typeof(Status), s));
            });

            modelBuilder.Entity<Quiz>(b =>
            {
                b.ToContainer("Quiz");
                b.HasKey(q => q.Id);
                b.Property(q => q.Id).ToJsonProperty("id");
                b.HasPartitionKey(q => q.CourseId);

                b.HasOne(q => q.AIModule)
                .WithOne(m => m.Quiz)
                .HasForeignKey<AIModule>(q => q.QuizId);

                b.Property(q => q.Status).HasConversion(
                    s => s.ToString(),
                    s => s != null ? (Status)Enum.Parse(typeof(Status), s) : default);
            });

            modelBuilder.Entity<Question>(b =>
            {
                b.ToContainer("Questions");
                b.HasKey(q => q.Id);
                b.Property(q => q.Id).ToJsonProperty("id");
                b.HasPartitionKey(q => q.QuizId);
                b.Property(q => q.Type).HasConversion(
                    qt => qt.ToString(),
                    qt => (QuestionType)Enum.Parse(typeof(QuestionType), qt));
            });
        }
    }
}
