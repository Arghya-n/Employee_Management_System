using System.Security.Claims;
using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.DTOModels;
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
        private readonly ILogger<EmployeeController> _logger;

        public EmployeeController(AppDBContext context, ILogger<EmployeeController> logger)
        {
            this.context = context;
            _logger= logger;
        }


        [Authorize(Roles = "Admin")]

        [HttpGet]
        public async Task<ActionResult> GetEmployees()
        {
            _logger.LogInformation("GetEmployees endpoint called by user: {User}", User.Identity?.Name);

            try
            {
                _logger.LogDebug("Fetching all employees from the database.");

                // Retrieves all employees from the database
                var data = await context.Employees.ToListAsync();

                if (data == null || data.Count == 0)
                {
                    _logger.LogWarning("No employee data found in the database.");
                    return NotFound("Data not found.");
                }

                _logger.LogInformation("Successfully retrieved {EmployeeCount} employees from the database.", data.Count);
                List<EmployeeDTO> employeesDTO = new List<EmployeeDTO>();
                foreach(Employee employee in context.Employees.ToList())
                {
                    EmployeeDTO employeeDTO = new EmployeeDTO();
                    employeeDTO.EmployeeId= employee.EmployeeId;
                    employeeDTO.Name= employee.Name;
                    employeeDTO.Email= employee.Email;
                    employeeDTO.Stack= employee.Stack;
                    employeeDTO.Role= employee.Role;
                    employeesDTO.Add(employeeDTO);
                }
                return Ok(employeesDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching employees from the database.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }






        // GET: api/Employee/5
        [Authorize(Roles = "Admin,User")]
        [HttpGet("{employeeId}")]
        public async Task<ActionResult> GetEmployeeById(int employeeId)
        {
            _logger.LogInformation("GetEmployeeById endpoint called by user: {User}, requesting employeeId: {RequestedEmployeeId}", User.Identity?.Name, employeeId);

            try
            {
                var loggedInEmployeeId = int.Parse(User.FindFirst("EmployeeId")?.Value);
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

                _logger.LogDebug("Logged-in user details - Role: {Role}, EmployeeId: {LoggedInEmployeeId}", userRole, loggedInEmployeeId);

                // Check if the user has access
                if (userRole != "Admin" && loggedInEmployeeId != employeeId)
                {
                    _logger.LogWarning("Unauthorized access attempt by user: {User} for employeeId: {RequestedEmployeeId}", User.Identity?.Name, employeeId);
                    return Forbid(); // Return 403 Forbidden if the user is not authorized
                }

                // Retrieve the employee from the database
                _logger.LogDebug("Attempting to retrieve employee with Id: {EmployeeId} from the database.", employeeId);
                var employee = await context.Employees.FindAsync(employeeId);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with Id: {EmployeeId} not found.", employeeId);
                    return NotFound(); // Return 404 if the employee does not exist
                }

                _logger.LogInformation("Successfully retrieved employee with Id: {EmployeeId}.", employeeId);
                EmployeeDTO employeeDTO = new EmployeeDTO();
                employeeDTO.EmployeeId = employee.EmployeeId;
                employeeDTO.Name = employee.Name;
                employeeDTO.Email = employee.Email;
                employeeDTO.Stack = employee.Stack;
                employeeDTO.Role = employee.Role;
                return Ok(employeeDTO); // Return the employee details
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing GetEmployeeById for employeeId: {EmployeeId}.", employeeId);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult> PostEmployee(EmployeePostDTO employeeDTO)
        {
            _logger.LogInformation("PostEmployee endpoint called by user: {User}", User.Identity?.Name);

            try
            {
                _logger.LogDebug("Generating salt for the new employee.");
                var salt = PasswordHasher.GenerateSalt();

                _logger.LogDebug("Hashing the password for the new employee.");
                // Hash the employee's password
                var hashedPassword = PasswordHasher.HashPassword(employeeDTO.Password, salt);
                
                Employee employee = new Employee();

                // Update the employee object
                _logger.LogDebug("Updating the employee object with hashed password, default role, and salt.");
                employee.Password = hashedPassword; // Store the hashed password
                employee.Role = employeeDTO.Role ?? "User"; // Default role if none provided
                employee.Salt = salt; // Store the salt for later password verification
                employee.Name= employeeDTO.Name;
                employee.Email= employeeDTO.Email;
                employee.Stack = employeeDTO.Stack;
               
                // Save the employee to the database
                _logger.LogDebug("Adding the employee to the database.");
                context.Employees.Add(employee);
                await context.SaveChangesAsync();

                _logger.LogInformation("Employee with Id: {EmployeeId} added successfully.", employee.EmployeeId);
                return Ok("Employee added successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding a new employee.");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }




        // PUT: api/Employee/5
        [Authorize(Roles = "Admin")]
        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployee(int employeeId, EmployeePostDTO updatedEmployee)
        {
            _logger.LogInformation("UpdateEmployee endpoint called by user: {User} to update employee with Id: {EmployeeId}", User.Identity?.Name, employeeId);

            try
            {
                _logger.LogDebug("Fetching employee with Id: {EmployeeId} from the database.", employeeId);
                var employee = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);

                if (employee == null)
                {
                    _logger.LogWarning("Employee with Id: {EmployeeId} not found.", employeeId);
                    return NotFound("Employee data not found.");
                }

                _logger.LogDebug("Generating salt and hashing password for employee with Id: {EmployeeId}.",employeeId);
                var salt = PasswordHasher.GenerateSalt();
                var hashedPassword = PasswordHasher.HashPassword(updatedEmployee.Password, salt);

                _logger.LogDebug("Updating employee properties for employee with Id: {EmployeeId}.", employeeId);
                employee.Password = hashedPassword;
                employee.Salt = salt;
                employee.Name = updatedEmployee.Name;
                employee.Email = updatedEmployee.Email;
                employee.Stack = updatedEmployee.Stack;
                employee.Role = updatedEmployee.Role;

                _logger.LogDebug("Updating employee record in the database for employee with Id: {EmployeeId}.", employeeId);
                context.Employees.Update(employee);
                await context.SaveChangesAsync();

                _logger.LogInformation("Successfully updated employee with Id: {EmployeeId}.", employeeId);
                return Ok(updatedEmployee);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating employee with Id: {EmployeeId}.", employeeId);
                return BadRequest("Error updating employee.");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            _logger.LogInformation("DeleteEmployee endpoint called by user: {User} to delete employee with Id: {EmployeeId}", User.Identity?.Name, id);

            try
            {
                _logger.LogDebug("Fetching employee with Id: {EmployeeId} from the database.", id);
                var data = await context.Employees.FirstOrDefaultAsync(x => x.EmployeeId == id);

                if (data == null)
                {
                    _logger.LogWarning("Employee with Id: {EmployeeId} not found.", id);
                    return NotFound();
                }

                _logger.LogDebug("Removing employee with Id: {EmployeeId} from the database.", id);
                context.Employees.Remove(data);
                await context.SaveChangesAsync();

                _logger.LogInformation("Successfully deleted employee with Id: {EmployeeId}.", id);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting employee with Id: {EmployeeId}.", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }



    }
}
