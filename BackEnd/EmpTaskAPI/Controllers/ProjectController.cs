
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
        private readonly ILogger<ProjectController> _logger;

        public ProjectController(AppDBContext context, ILogger<ProjectController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetProjects()
        {
            _logger.LogInformation("GetProjects endpoint called.");
            var data = await context.Projects.ToListAsync();

            if (data == null)
            {
                _logger.LogWarning("No projects found.");
                return NotFound("Data not found.");
            }

            _logger.LogInformation("Successfully retrieved all projects.");
            return Ok(data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetProjectById(int employeeId)
        {
            _logger.LogInformation("GetProjectById endpoint called by user: {User} for employeeId: {EmployeeId}.", User.Identity?.Name, employeeId);

            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var assignedTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId==employeeId);
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignedTask.TaskId);
            var projectf = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == task.ProjectId);

            if (projectf == null)
            {
                _logger.LogWarning("Project not found or unauthorized access for employeeId: {EmployeeId}.", employeeId);
                return Unauthorized();
            }

            if (userRole != "Admin" && employeeId != loggedInEmployeeId)
            {
                _logger.LogWarning("Unauthorized access by user: {User} for employeeId: {EmployeeId}.", User.Identity?.Name, employeeId);
                return Unauthorized();
            }

            _logger.LogInformation("Successfully retrieved project for employeeId: {EmployeeId}.", employeeId);
            return Ok(projectf);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostProject(Project project)
        {
            _logger.LogInformation("PostProject endpoint called by user: {User}.", User.Identity?.Name);

            context.Projects.Add(project);
            await context.SaveChangesAsync();

            _logger.LogInformation("Successfully created new project with title: {Title}.", project.Title);
            return Ok("Done");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Project updatedProject)
        {
            _logger.LogInformation("UpdateUser endpoint called by user: {User} for projectId: {ProjectId}.", User.Identity?.Name, id);

            if (id == null)
            {
                _logger.LogWarning("UpdateUser called with null projectId.");
                return BadRequest();
            }

            var project = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if (project == null)
            {
                _logger.LogWarning("Project with projectId: {ProjectId} not found.", id);
                return NotFound("Project Data not found.");
            }

            project.StartDate = updatedProject.StartDate;
            project.EndDate = updatedProject.EndDate;
            project.Title = updatedProject.Title;
            project.Description = updatedProject.Description;

            await context.SaveChangesAsync();

            _logger.LogInformation("Successfully updated project with projectId: {ProjectId}.", id);
            return Ok(project);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteProject(int id)
        {
            _logger.LogInformation("DeleteProject endpoint called by user: {User} for projectId: {ProjectId}.", User.Identity?.Name, id);

            if (id == null)
            {
                _logger.LogWarning("DeleteProject called with null projectId.");
                return BadRequest();
            }

            var data = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
            if (data == null)
            {
                _logger.LogWarning("Project with projectId: {ProjectId} not found.", id);
                return NotFound();
            }

            context.Projects.Remove(data);
            var data1 = context.Tasks.Where(t => t.ProjectId == id);
            context.Tasks.RemoveRange(data1);
            await context.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted project with projectId: {ProjectId}.", id);
            return Ok("Successfully Deleted!");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("all")]
        public async Task<IActionResult> DeleteAllProjects()
        {
            _logger.LogInformation("DeleteAllProjects endpoint called by user: {User}.", User.Identity?.Name);

            var transaction = await context.Database.BeginTransactionAsync();
            try
            {
                var projectIds = await context.Projects.Select(p => p.ProjectId).ToListAsync();
                var taskIds = await context.Tasks.Where(t => projectIds.Contains(t.ProjectId)).Select(t => t.TaskId).ToListAsync();

                _logger.LogDebug("Deleting assigned tasks.");
                context.AssignedTasks.RemoveRange(context.AssignedTasks.Where(at => taskIds.Contains(at.TaskId)));

                _logger.LogDebug("Deleting tasks.");
                context.Tasks.RemoveRange(context.Tasks.Where(t => projectIds.Contains(t.ProjectId)));

                _logger.LogDebug("Deleting projects.");
                context.Projects.RemoveRange(context.Projects.Where(p => projectIds.Contains(p.ProjectId)));

                await context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Successfully deleted all projects and related tasks.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting all projects.");
                await transaction.RollbackAsync();
                throw;
            }

            return Ok("Deleted.");
        }
    }
}
