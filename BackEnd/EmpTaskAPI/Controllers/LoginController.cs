using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.HashPassword;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EmpTaskAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;

        public LoginController(AppDBContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<IActionResult> GetToken(Employee emp)
        {
            // Authenticate the user
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == emp.Email);
            if (employee == null)
            {
                return Unauthorized("Invalid email");
            }

            var isValidPassword = PasswordHasher.HashPassword(emp.Password, employee.Salt) == employee.Password;

            if (!isValidPassword)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate a JWT token
            string token = GenerateToken(employee);

            // Return the token along with EmployeeId and Role
            return Ok(new
            {
                Token = token,
                EmployeeId = employee.EmployeeId,
                Role = employee.Role
            });
        }

        private string GenerateToken(Employee emp)
        {
            // Create the security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define claims for the token
            var claims = new List<Claim>
            {

                new Claim(ClaimTypes.Name, emp.Name),
                new Claim(ClaimTypes.Role, emp.Role), // Role-based claim
                new Claim("EmployeeId", emp.EmployeeId.ToString()) // Custom claim for Employee ID

            };

            // Create the token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
