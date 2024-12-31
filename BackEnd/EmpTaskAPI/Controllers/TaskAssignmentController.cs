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
    public class TaskAssignmentController : ControllerBase
    {
        private readonly AppDBContext context;
        private readonly ILogger<TaskAssignmentController> _logger;

        public TaskAssignmentController(AppDBContext context, ILogger<TaskAssignmentController> logger)
        {
            this.context = context;
            _logger = logger;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Fetching all assigned tasks");
            var data = await context.AssignedTasks.ToListAsync();
            _logger.LogInformation("Fetched {count} assigned tasks", data.Count);
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TaskAssignment ta)
        {
            _logger.LogInformation("Creating new task assignment for employee {employeeId}", ta.EmployeeId);
            await context.AssignedTasks.AddAsync(ta);
            await context.SaveChangesAsync();
            _logger.LogInformation("Task assignment created with ID {assignmentId}", ta.Id);
            return Ok(ta);
        }

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetTaskAssignmentById(int employeeId)
        {
            _logger.LogInformation("Fetching task assignment for employee {employeeId}", employeeId);
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

            if (data == null)
            {
                _logger.LogWarning("Task assignment not found for employee {employeeId}", employeeId);
                return NotFound();
            }

            if (userRole != "Admin" && employeeId != loggedInEmployeeId)
            {
                _logger.LogWarning("Employee {employeeId} is not authorized to access this resource", loggedInEmployeeId);
                return Unauthorized();
            }

            _logger.LogInformation("Task assignment found for employee {employeeId}: {assignmentId}", employeeId, data.Id);
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AssignTask(int id, TaskAssignment ta)
        {
            _logger.LogInformation("Updating task assignment with ID {assignmentId}", id);
            if (id == null)
            {
                _logger.LogWarning("Invalid task assignment ID {assignmentId}", id);
                return BadRequest();
            }

            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                _logger.LogWarning("Task assignment with ID {assignmentId} not found", id);
                return NotFound();
            }

            data.EmployeeId = ta.EmployeeId;
            data.TaskId = ta.TaskId;
            await context.SaveChangesAsync();
            _logger.LogInformation("Task assignment with ID {assignmentId} updated successfully", id);
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            _logger.LogInformation("Deleting task assignment with ID {assignmentId}", id);
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                _logger.LogWarning("Task assignment with ID {assignmentId} not found", id);
                return NotFound();
            }

            context.AssignedTasks.Remove(data);
            await context.SaveChangesAsync();
            _logger.LogInformation("Task assignment with ID {assignmentId} deleted", id);
            return Ok(data);
        }





    }
}
