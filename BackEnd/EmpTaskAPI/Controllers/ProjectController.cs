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
        // GET: api/ProjectsTasks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Project>>> GetProjects()
        {
            // Retrieves all projects with their related tasks
            var data = await context.Projects.Include(u => u.Tasks).ToListAsync();

            if (data == null)
                return NotFound("Data not found.");

            return Ok(data);
           // return await context.Projects.Include(p => p.Tasks).ToListAsync();

        }

        // GET: api/ProjectsTasks/5
        [HttpGet("{projectId}")]
        public async Task<ActionResult<Project>> GetProjectById(int projectId)
        {
            // Retrieves a single project by ID with its related tasks
            var project = await context.Projects
                .Include(p => p.Tasks)
                .FirstOrDefaultAsync(p => p.ProjectId == projectId);

            if (project == null)
            {
                return NotFound();
            }

            return project;
        }

        // POST: api/ProjectsTasks/Project
        //[HttpPost("Project")]
        //public async Task<ActionResult<Project>> PostProject(Project project)
        //{
        //    context.Projects.Add(project);
        //    await context.SaveChangesAsync();

        //    // Return the created project with a 201 status code
        //    //return CreatedAtAction(nameof(GetProjectById), new { projectId = project.ProjectId }, project);
        //    return Ok("Done");
        //}
        [HttpPost("Project")]
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


        // POST: api/ProjectsTasks/Task
        [HttpPost("Task")]
        public async Task<ActionResult<Models.Task>> PostTask(Models.Task task)
        {
            // Ensure the project exists before adding a task
            var project = await context.Projects.FindAsync(task.ProjectId);
            if (project == null)
            {
                return NotFound($"Project with ID {task.ProjectId} not found.");
            }

            context.Tasks.Add(task);
            await context.SaveChangesAsync();

            // Return the created task with a 201 status code
            return CreatedAtAction(nameof(GetProjectById), new { projectId = task.ProjectId }, task);
        }

    }
}
