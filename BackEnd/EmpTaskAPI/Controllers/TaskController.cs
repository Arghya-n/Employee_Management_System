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
        private readonly ILogger<TaskController> _logger;

        public TaskController(AppDBContext context, ILogger<TaskController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves a list of all tasks.
        /// Requires Admin role.
        /// </summary>
        /// <returns>A list of tasks.</returns>

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all tasks");
            var data = await context.Tasks.ToListAsync();
            _logger.LogInformation("Fetched {count} tasks", data.Count);
            return Ok(data);
        }


        /// <summary>
        /// Retrieves the task details for a specific employee.
        /// Requires Admin or User role.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>The task details if found and authorized, otherwise Forbidden or NotFound result.</returns>

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


        /// <summary>
        /// Creates a new task.
        /// Requires Admin role.
        /// </summary>
        /// <param name="ts">The task object containing task details.</param>
        /// <returns>The created task object.</returns>

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


        /// <summary>
        /// Deletes a specific task.
        /// Requires Admin role.
        /// </summary>
        /// <param name="id">The ID of the task to delete.</param>
        /// <returns>The deleted task object if successful, otherwise NotFound result.</returns>

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



        /// <summary>
        /// Updates the details of a specific task.
        /// Requires Admin or User role.
        /// </summary>
        /// <param name="id">The ID of the task to update.</param>
        /// <param name="uts">The updated task object containing new details.</param>
        /// <returns>The updated task object if successful, otherwise NotFound or Forbidden result.</returns>

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
