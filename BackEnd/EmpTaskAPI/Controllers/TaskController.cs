using EmpTaskAPI.DataAccessLayer;
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


        
    }
}
