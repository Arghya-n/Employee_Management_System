using System.Security.Claims;
using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpTaskAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly AppDBContext context;

        public ProjectController(AppDBContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetProjects()
        {
            // Retrieves all projects with their related tasks
            var data = await context.Projects.ToListAsync();

            if (data == null)
                return NotFound("Data not found.");

            return Ok(data);
          

        }

        // GET: api/ProjectsTasks/5
        [Authorize(Roles ="Admin,User")]
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetProjectById(int projectId)
        {
            // Retrieves a single project by ID with its related tasks
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var assignedTask =await context.AssignedTasks.FirstOrDefaultAsync(x=>x.EmployeeId==loggedInEmployeeId);
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignedTask.TaskId);
            var projectf = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == task.ProjectId);
            
            if (projectf == null)
            {
                return Unauthorized();
            }
            if (userRole != "Admin" && projectf.ProjectId!=projectId ) {
                return Unauthorized();
            }
            var project = await context.Projects.FindAsync(projectId);
                

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost]
        public async Task<ActionResult> PostProject(Project project)
        {

            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return Ok("Done");

            // Return the created project along with its tasks
           // return CreatedAtAction(nameof(GetProjectById), new { projectId = project.ProjectId }, project);
        }



        // PUT: api/ProjectsTasks/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Project updatedProject)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var project = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if (project == null)
                return NotFound("Project Data not found.");
           

            project.StartDate = updatedProject.StartDate;
            project.EndDate = updatedProject.EndDate;
            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;

            
            await context.SaveChangesAsync();


            return Ok(project);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var data = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if(data == null)
            {
                return NotFound();
            }

            context.Projects.Remove(data);
            var data1=  context.Tasks.Where(t => t.ProjectId == id);
            context.Tasks.RemoveRange(data1);
            await context.SaveChangesAsync();
            return Ok("Successfully Deleted!");

        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllProjects()
        {
            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Fetch the Project IDs
                var projectIds = await context.Projects
                                              .Select(p => p.ProjectId)
                                              .ToListAsync();

                // Fetch the Task IDs
                var taskIds = await context.Tasks
                                           .Where(t => projectIds.Contains(t.ProjectId))
                                           .Select(t => t.TaskId)
                                           .ToListAsync();

                // Delete AssignedTasks
                context.AssignedTasks.RemoveRange(
                    context.AssignedTasks.Where(at => taskIds.Contains(at.TaskId))
                );

                // Delete Tasks
                context.Tasks.RemoveRange(
                    context.Tasks.Where(t => projectIds.Contains(t.ProjectId))
                );

                // Delete Projects
                context.Projects.RemoveRange(
                    context.Projects.Where(p => projectIds.Contains(p.ProjectId))
                );

                // Save changes and commit transaction
                await context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
            return Ok("Deleted.");
        }

    }
}
