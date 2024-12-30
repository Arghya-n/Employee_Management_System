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

        public TaskController(AppDBContext context) {
            this.context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var data = await context.Tasks.ToListAsync();

            return Ok(data);
        }
        //[Authorize(Roles = "Admin")]
        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get(int id)
        //{

        //    var data = await context.Tasks.FirstOrDefaultAsync(x=>x.TaskId==id);
        //    return Ok(data);
        //}
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmploeeById(int employeeId)
        {
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Check if the user has access
            if (userRole != "Admin" && loggedInEmployeeId != employeeId)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }

            // Retrieve the employee from the database
         //   var assignTask = await context.AssignedTasks.FindAsync(employeeId);
            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            if (assignTask == null)
            {
                return NotFound(); // Return 404 if the employee does not exist
            }
            //var task = await context.Tasks.FindAsync(assignTask.TaskId);
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);

            if (task == null)
            {
                return NotFound(); // Return 404 if the employee does not exist
            }

            return Ok(task); // Return the employee details
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task ts)
        {
            await context.Tasks.AddAsync(ts);
            await context.SaveChangesAsync();
            return Ok(ts);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var data = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
           
            if (data == null)
            {
                return NotFound();
            }
            context.Tasks.Remove(data);
            await context.SaveChangesAsync();
            return Ok(data);

        }
        [Authorize(Roles = "Admin,User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.Task uts)
        {
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var assignTask = await context.AssignedTasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (assignTask == null)
            {
                return NotFound(); // Return 404 if the employee does not exist
            }

            // Check if the user has access
            if (userRole != "Admin" && loggedInEmployeeId != assignTask.EmployeeId)

            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            //if (employeeId == null&& userRole != "Admin")
            //{
            //    return BadRequest();
            //}
            
            //var task = await context.Tasks.FindAsync(assignTask.TaskId);
            var task = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == assignTask.TaskId);

            if (task == null)
            {
                return NotFound(); // Return 404 if the employee does not exist
            }

            task.Status =uts.Status;
            task.SubmitDate = uts.SubmitDate;
            task.AssignDate = uts.AssignDate;
            task.Comments = uts.Comments;
            task.ProjectId=uts.ProjectId;
            
            await context.SaveChangesAsync();
            return Ok(task);
        }


    }
}
