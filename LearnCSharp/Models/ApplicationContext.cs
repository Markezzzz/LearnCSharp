using LearnCSharp.Models.Course;
using LearnCSharp.Models.Users;
using Microsoft.EntityFrameworkCore;

namespace LearnCSharp.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Course.Course> Courses { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Chapter> Chapters { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chapter>()
                .HasOne<Module>(c => c.Module)
                .WithMany(m => m.Chapters)
                .HasForeignKey(c => c.ModuleId)
                .OnDelete(DeleteBehavior.Cascade);
            
            modelBuilder.Entity<Module>()
                .HasOne<Course.Course>(m => m.Course)
                .WithMany(c => c.Modules)
                .HasForeignKey(m => m.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}