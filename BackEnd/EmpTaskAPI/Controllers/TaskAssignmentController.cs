﻿using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
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
        [HttpGet]
        public async Task<IActionResult> Get() {
            var data = await context.AssignedTasks.ToListAsync();
            return Ok(data);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TaskAssignment ta)
        {
            await context.AssignedTasks.AddAsync(ta);
            await context.SaveChangesAsync();
            return Ok(ta);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskAssignmentById(int id)
        {
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();
            }

            return Ok(data);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> AssignTask(int id,TaskAssignment ta)
        {
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x=>x.Id==id);
            if (data == null)
            {
                return NotFound();

            }
            context.AssignedTasks.Update(data);
            context.SaveChangesAsync();
            return Ok(ta);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAssignment(int id)
        {
            var data = await context.AssignedTasks.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null)
            {
                return NotFound();

            }
            context.AssignedTasks.Remove(data);
            context.SaveChangesAsync();

            return Ok(data);
        }
        

        

    }
}