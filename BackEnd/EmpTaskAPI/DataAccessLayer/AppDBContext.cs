using EmpTaskAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmpTaskAPI.DataAccessLayer
{
    public class AppDBContext: DbContext
    {
        public AppDBContext(DbContextOptions options) :base(options){

        }
        public DbSet<Project> Projects { get; set; }

        public DbSet<Models.Task> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the one-to-many relationship (Project -> Tasks)
            modelBuilder.Entity<Project>()
                .HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId);
        }

    }
}
