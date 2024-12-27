using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        [HttpGet]
        public async Task<IActionResult> Get() {
            var data = await context.Tasks.ToListAsync();

            return Ok(data);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var data = await context.Tasks.FirstOrDefaultAsync(x=>x.TaskId==id);
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Models.Task ts)
        {
            await context.Tasks.AddAsync(ts);
            await context.SaveChangesAsync();
            return Ok(ts);
        }
        [HttpDelete]
        public async Task<IActionResult> DeleteTask(int id)
        {
            var data = await context.Tasks.FirstOrDefaultAsync(x => x.TaskId == id);
            if (data == null)
            {
                return NotFound();
            }
            context.Tasks.Remove(data);
            context.SaveChangesAsync();
            return Ok(data);

        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Models.Task uts)
        {
            using var transaction = await context.Database.BeginTransactionAsync();

            try
            {
                var project = await context.Projects.FirstOrDefaultAsync(x => x.ProjectId == id);
                if (project == null)
                    return NotFound("Project Data not found.");

                context.Projects.Update(project);
                await context.SaveChangesAsync();
                return Ok(project);
            }
            catch
            {
                await transaction.RollbackAsync();
                return BadRequest("Error updating user.");
            }
        }


    }
}
