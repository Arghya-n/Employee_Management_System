using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
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
        
        [HttpGet]
        public async Task<ActionResult> GetProjects()
        {
            // Retrieves all projects with their related tasks
            var data = await context.Projects.ToListAsync();

            if (data == null)
                return NotFound("Data not found.");

            return Ok(data);
           // return await context.Projects.Include(p => p.Tasks).ToListAsync();

        }

        // GET: api/ProjectsTasks/5
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetProjectById(int projectId)
        {
            // Retrieves a single project by ID with its related tasks
            var project = await context.Projects.FindAsync(projectId);
                

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
<<<<<<< HEAD

        
       
=======
        [HttpPost]
        public async Task<ActionResult<Project>> PostProject(Project project)
        {
            if (project.Tasks != null && project.Tasks.Count > 0)
            {
                // Set the ProjectId for each task (to associate them with the project)
                foreach (var task in project.Tasks)
                {
                    task.ProjectId = project.ProjectId;  // Assign the ProjectId to each task
                    context.Tasks.Add(task);
                }
            }

            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return Ok("DOne");

            // Return the created project along with its tasks
           // return CreatedAtAction(nameof(GetProjectById), new { projectId = project.ProjectId }, project);
        }


       
        // PUT: api/ProjectsTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] Project updatedProject)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var project = await context.Projects.Include(u => u.Tasks).FirstOrDefaultAsync(u => u.ProjectId == id);

                if (project == null)
                    return NotFound("Project Data not found.");

                // Update user details
                project.Title = updatedProject.Title;
                project.Description = updatedProject.Description;
                project.StartDate = updatedProject.StartDate;
                project.EndDate = updatedProject.EndDate;

                // Update profile details
                if (project.Tasks != null && updatedProject.Tasks != null)
                {
                    foreach (var updatedTask in updatedProject.Tasks)
                    {
                        // Check if the task already exists in the project
                        var existingTask = project.Tasks.FirstOrDefault(t => t.TaskId == updatedTask.TaskId);

                        if (existingTask != null)
                        {
                            // Update the existing task
                            existingTask.AssignDate = updatedTask.AssignDate;
                            existingTask.SubmitDate = updatedTask.SubmitDate;
                            existingTask.Status = updatedTask.Status;
                        }
                        else
                        {
                            // Add the new task to the project
                            project.Tasks.Add(new Models.Task
                            {
                                TaskId = updatedTask.TaskId,
                                AssignDate = updatedTask.AssignDate,
                                SubmitDate = updatedTask.SubmitDate,
                                Status = updatedTask.Status,
                                ProjectId = project.ProjectId // Ensure the ProjectId is set correctly
                            });
                        }
                    }
                }



                context.Projects.Update(project);
                await context.SaveChangesAsync();

                await transaction.CommitAsync();
                return Ok(project);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest("Error updating user.");
            }
        }
        // DELETE: api/ProjectsTasks/5
        [HttpDelete("{projectId}")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            // Check if the project exists
            var project = await context.Projects.Include(p => p.Tasks).FirstOrDefaultAsync(p => p.ProjectId == projectId);
            if (project == null)
            {
                return NotFound($"Project with ID {projectId} not found.");
            }

            // Remove related tasks
            context.Tasks.RemoveRange(project.Tasks);

            // Remove the project
            context.Projects.Remove(project);

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return BadRequest("Failed to delete the project.");
            }

            return Ok("Project deleted successfully.");
        }
>>>>>>> 1a53cef043d1830c9f0f7eaa089cc34811ffd128

    }
}
