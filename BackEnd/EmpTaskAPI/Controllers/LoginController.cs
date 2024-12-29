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
        //[HttpPost]
        //public IActionResult GetToken(Employee employee)
        //{
        //    var data = _context.Employees.ToList();
        //    string token = "";
        //    if (data[0].Name.Equals(employee.Name) && data[0].Password.Equals(employee.Password))
        //    {
        //        token = GenerateToken(employee);

        //    }
        //    return Ok(token);
        //}
        //private string GenerateToken(Employee emp)
        //{

        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null, expires: DateTime.Now.AddMinutes(1), signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        [HttpPost]
        public IActionResult GetToken(Employee employee)
        {
            var data = _context.Employees.FirstOrDefault(e => e.Name.Equals(employee.Name) && e.Password.Equals(employee.Password));
            if (data == null)
            {
                return Unauthorized("Invalid Employee ID or Password.");
            }

            // Check the role of the employee
            string token = GenerateToken(data);
            return Ok(new { Token = token, Role = data.Role });
        }

        private string GenerateToken(Employee emp)
        {
            // Create the security key
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Define role-specific claims
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, emp.Name.ToString()),
        new Claim(ClaimTypes.Role, emp.Role) // Add role claim
    };

            // Create the token with role-based claims
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
