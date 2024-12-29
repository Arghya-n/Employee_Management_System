using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Mvc;
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
        public IActionResult GetToken(Employee employee)
        {
            // Authenticate the user
            var data = _context.Employees
                .FirstOrDefault(e => e.Name.Equals(employee.Name) && e.Password.Equals(employee.Password));

            if (data == null)
            {
                return Unauthorized("Invalid Employee ID or Password.");
            }

            // Generate a JWT token
            string token = GenerateToken(data);

            // Return the token along with EmployeeId and Role
            return Ok(new
            {
                Token = token,
                EmployeeId = data.EmployeeId,
                Role = data.Role
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
<<<<<<< HEAD
                new Claim(ClaimTypes.Name, emp.Name),
                new Claim(ClaimTypes.Role, emp.Role), // Role-based claim
                new Claim("EmployeeId", emp.EmployeeId.ToString()) // Custom claim for Employee ID
=======
                new Claim(ClaimTypes.Name, emp.Name.ToString()),
                new Claim(ClaimTypes.Role, emp.Role) // Add role claim
>>>>>>> 1fdfae1d9510441a507371aa7f0fae75b4af260d
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
