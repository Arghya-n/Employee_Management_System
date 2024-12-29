using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpTaskAPI.Controllers
{
    [Authorize(Roles = "Admin")]
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
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllProjects()
        {
            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                // Fetch project IDs
                var projectIds = await context.Projects
                                              .Select(p => p.ProjectId)
                                              .ToListAsync();

                // Delete tasks
                context.Tasks.RemoveRange(context.Tasks.Where(t => projectIds.Contains(t.ProjectId)));

                // Delete projects
                context.Projects.RemoveRange(context.Projects.Where(p => projectIds.Contains(p.ProjectId)));

                // Save changes
                await context.SaveChangesAsync();

                // Commit the transaction
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
