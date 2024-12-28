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
        public DbSet<TaskAssignment> AssignedTasks { get; set; }

        public DbSet<Employee> Employees { get; set; }

        

    }
}
