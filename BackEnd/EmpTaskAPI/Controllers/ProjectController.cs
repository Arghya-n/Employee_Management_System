﻿using EmpTaskAPI.DataAccessLayer;
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

        [HttpPost]
        public async Task<ActionResult> PostProject(Project project)
        {

            context.Projects.Add(project);
            await context.SaveChangesAsync();
            return Ok("Done");

            // Return the created project along with its tasks
           // return CreatedAtAction(nameof(GetProjectById), new { projectId = project.ProjectId }, project);
        }


       
        // PUT: api/ProjectsTasks/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, Project updatedProject)
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
