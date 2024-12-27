using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
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

        public ProjectController(AppDBContext context)
        {
            this.context = context;
        }
        
        [HttpGet]
        public async Task<ActionResult> GetProjects()
        {
            // Retrieves all projects with their related tasks
            var data = await context.Projects.ToListAsync();

            if (data == null)
                return NotFound("Data not found.");

            return Ok(data);
           // return await context.Projects.Include(p => p.Tasks).ToListAsync();

        }

        // GET: api/ProjectsTasks/5
        [HttpGet("{projectId}")]
        public async Task<ActionResult> GetProjectById(int projectId)
        {
            // Retrieves a single project by ID with its related tasks
            var project = await context.Projects.FindAsync(projectId);
                

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }

        
       

    }
}
