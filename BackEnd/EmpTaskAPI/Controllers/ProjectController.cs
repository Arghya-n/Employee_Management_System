using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public IActionResult Get()
        {
            var data = context.Projects.ToList();

            return Ok(data);
        }
        [HttpPost]

        public async Task<IActionResult> Create(Project pj)
        {
            context.AddAsync(pj);
            context.SaveChangesAsync();
            return Ok(pj);

        }

    }
}
