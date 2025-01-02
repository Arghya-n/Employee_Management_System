using EmpTaskAPI.DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
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

            // Fetch all tasks
            var tasks = await context.Tasks.ToListAsync();

            // Transform tasks to include formatted dates
            var formattedTasks = tasks.Select(task => new
            {
                task.TaskId,
                task.ProjectId,
                AssignDate = task.AssignDate.ToString("dd MMM, yyyy"),
                SubmitDate = task.SubmitDate?.ToString("dd MMM, yyyy"),
                task.Status,
                task.Comments
            });

            return Ok(formattedTasks);
        }


        /// <summary>
        /// Retrieves the task details for a specific employee.
        /// Requires Admin or User role.
        /// </summary>
        /// <param name="employeeId">The ID of the employee.</param>
        /// <returns>The task details if found and authorized, otherwise Forbidden or NotFound result.</returns>

        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmployeeById(int employeeId)
        {
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            if (userRole != "Admin" && loggedInEmployeeId != employeeId)
            {
                return Forbid();
            }

            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (assignTask == null) return NotFound();

            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);
            if (task == null) return NotFound();

            // Transform the task into a DTO with formatted date
            var taskDTO = new
            {
                task.TaskId,
                task.ProjectId,
                AssignDate = task.AssignDate.ToString("dd MMM, yyyy"),
                SubmitDate = task.SubmitDate?.ToString("dd MMM, yyyy"),
                task.Status,
                task.Comments
            };

            return Ok(taskDTO);
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
            await context.Tasks.AddAsync(ts);
            await context.SaveChangesAsync();
            var taskDTO = new
            {
                ts.TaskId,
                ts.ProjectId,
                AssignDate = ts.AssignDate.ToString("dd MMM, yyyy"),
                SubmitDate = ts.SubmitDate?.ToString("dd MMM, yyyy"),
                ts.Status,
                ts.Comments
            };
            return Ok(taskDTO);
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
            var data = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (data == null) return NotFound();

            context.Tasks.Remove(data);
            await context.SaveChangesAsync();
            var taskDTO = new
            {
                data.TaskId,
                data.ProjectId,
                AssignDate = data.AssignDate.ToString("dd MMM, yyyy"),
                SubmitDate = data.SubmitDate?.ToString("dd MMM, yyyy"),
                data.Status,
                data.Comments
            };
            return Ok(taskDTO);
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
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (assignTask == null) return NotFound();

            if (userRole != "Admin" && loggedInEmployeeId != assignTask.EmployeeId)
            {
                return Forbid();
            }

            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);
            if (task == null) return NotFound();

            task.Status = uts.Status;
            task.SubmitDate = uts.SubmitDate;
            task.AssignDate = uts.AssignDate;
            task.Comments = uts.Comments;
            task.ProjectId = uts.ProjectId;

            await context.SaveChangesAsync();
            var taskDTO = new
            {
                task.TaskId,
                task.ProjectId,
                AssignDate = task.AssignDate.ToString("dd MMM, yyyy"),
                SubmitDate = task.SubmitDate?.ToString("dd MMM, yyyy"),
                task.Status,
                task.Comments
            };
            return Ok(taskDTO);
        }
    }
}
