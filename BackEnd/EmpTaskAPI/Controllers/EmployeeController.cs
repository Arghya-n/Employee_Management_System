using System.Security.Claims;
using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.HashPassword;
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
            var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            // Check if the user has access
            if (userRole != "Admin" && loggedInEmployeeId != employeeId)
            {
                return Forbid(); // Return 403 Forbidden if the user is not authorized
            }
            // Retrieve the employee from the database
            var employee = await context.Employees.FindAsync(employeeId);

            if (employee == null)
            {
                return NotFound(); // Return 404 if the employee does not exist
            }

            return Ok(employee); // Return the employee details
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostEmployee(Employee employee)
        {

            var salt = PasswordHasher.GenerateSalt();

            // Hash the employee's password
            var hashedPassword = PasswordHasher.HashPassword(employee.Password, salt);

            // Update the employee object
            employee.Password = hashedPassword; // Store the hashed password
            employee.Role = employee.Role ?? "Employee"; // Default role if none provided
            employee.Salt = salt; // Store the salt for later password verification (you need to add this field to the Employee model)

            // Save the employee to the database
            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            return Ok("Employee added successfully");
        }



        // PUT: api/Employee/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, Employee updatedEmployee)
        {

            try
            {
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);

                if (employee == null)
                    return NotFound("Project Data not found.");
                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(updatedEmployee.Password, salt);
                employee.Password = hashedPassword;
                employee.Salt = salt;
                employee.Name = updatedEmployee.Name;
                employee.Email = updatedEmployee.Email;
                employee.Stack = updatedEmployee.Stack;
                employee.Role = updatedEmployee.Role;

                //employee.EmployeeId = updatedEmployee.EmployeeId;

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
            await context.SaveChangesAsync();
            return Ok(data);

        }


    }
}
