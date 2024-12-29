using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpTaskAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly AppDBContext context;

        public EmployeeController(AppDBContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            // Retrieves all projects with their related tasks
            var data = await context.Employees.ToListAsync();

            if (data == null)
                return NotFound("Data not found.");

            return Ok(data);


        }

        // GET: api/Employee/5
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmploeeById(int employeeId)
        {
            // Retrieves a single project by ID with its related tasks
            var employee = await context.Employees.FindAsync(employeeId);


            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostEmployee(Employee employee)
        {

            context.Employees.Add(employee);
            await context.SaveChangesAsync();
            return Ok("Done");
        }



        // PUT: api/Employee/5
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updatedEmployee)
        {

            try
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);

                if (employee == null)
                    return NotFound("Project Data not found.");

                employee.Password = updatedEmployee.Password;
                employee.Name = updatedEmployee.Name;
                employee.Email = updatedEmployee.Email;
                employee.Stack = updatedEmployee.Stack;
                employee.Role = updatedEmployee.Role;

                context.Employees.Update(employee);
                await context.SaveChangesAsync();

                return Ok(employee);
            }
            catch
            {
                return BadRequest("Error updating user.");
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var data = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);
            if (data == null)
            {
                return NotFound();
            }
            context.Employees.Remove(data);
            context.SaveChangesAsync();
            return Ok(data);

        }


    }
}
