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

        public TaskAssignmentController(AppDBContext context) {
            this.context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Get() {
            var data = await context.AssignedTasks.ToListAsync();
            return Ok(data);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(TaskAssignment ta)
        {
            await context.AssignedTasks.AddAsync(ta);
            await context.SaveChangesAsync();
            return Ok(ta);
        }
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<IActionResult> GetTaskAssignmentById(int employeeId)
        {
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

             var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
           // var data = await context.AssignedTasks.FindAsync(employeeId);

            if (data == null)
            {
                return NotFound();
            }

            if (userRole != "Admin" && employeeId != loggedInEmployeeId)
            {
                return Unauthorized();
            }

            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> AssignTask(int id,TaskAssignment ta)
        {
            if (id == null)
            {
                return BadRequest();
            }
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x=>x.Id==id);
            if (data == null)
            {
                return NotFound();

            }
            data.EmployeeId = ta.EmployeeId;
            data.TaskId = ta.TaskId;

            context.SaveChangesAsync();
            return Ok(data);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();

            }
            context.AssignedTasks.Remove(data);
            await context.SaveChangesAsync();

            return Ok(data);
        }
        

        

    }
}
