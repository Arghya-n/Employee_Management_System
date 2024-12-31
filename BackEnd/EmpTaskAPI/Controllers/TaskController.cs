using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EmpTaskAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private readonly AppDBContext context;
        private readonly ILogger<ProjectController> _logger;

        public TaskController(AppDBContext context, ILogger<ProjectController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all tasks");
            var data = await context.Tasks.ToListAsync();
            _logger.LogInformation("Fetched {count} tasks", data.Count);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmploeeById(int employeeId)
        {
            _logger.LogInformation("Fetching task for employee {employeeId}", employeeId);
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Admin" && loggedInEmployeeId != employeeId)
            {
                _logger.LogWarning("Employee {employeeId} is not authorized to access this resource", loggedInEmployeeId);
                return Forbid();
            }

            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (assignTask == null)
            {
                _logger.LogWarning("Assigned task not found for employee {employeeId}", employeeId);
                return NotFound();
            }

            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);
            if (task == null)
            {
                _logger.LogWarning("Task not found for assigned task {taskId}", assignTask.TaskId);
                return NotFound();
            }

            _logger.LogInformation("Task found for employee {employeeId}: {taskId}", employeeId, task.TaskId);
            return Ok(task);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task ts)
        {
            _logger.LogInformation("Creating new task with title {taskId}", ts.TaskId);
            await context.Tasks.AddAsync(ts);
            await context.SaveChangesAsync();
            _logger.LogInformation("Task created with ID {taskId}", ts.TaskId);
            return Ok(ts);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with ID {taskId}", id);
            var data = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (data == null)
            {
                _logger.LogWarning("Task with ID {taskId} not found", id);
                return NotFound();
            }
            context.Tasks.Remove(data);
            await context.SaveChangesAsync();
            _logger.LogInformation("Task with ID {taskId} deleted", id);
            return Ok(data);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.Task uts)
        {
            _logger.LogInformation("Updating task with ID {taskId}", id);
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (assignTask == null)
            {
                _logger.LogWarning("Assigned task not found for task ID {taskId}", id);
                return NotFound();
            }

            if (userRole != "Admin" && loggedInEmployeeId != assignTask.EmployeeId)
            {
                _logger.LogWarning("Employee {employeeId} is not authorized to update task {taskId}", loggedInEmployeeId, id);
                return Forbid();
            }

            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);
            if (task == null)
            {
                _logger.LogWarning("Task not found for task ID {taskId}", id);
                return NotFound();
            }

            task.Status = uts.Status;
            task.SubmitDate = uts.SubmitDate;
            task.AssignDate = uts.AssignDate;
            task.Comments = uts.Comments;
            task.ProjectId = uts.ProjectId;

            await context.SaveChangesAsync();
            _logger.LogInformation("Task with ID {taskId} updated successfully", id);
            return Ok(task);
        }



    }
}
