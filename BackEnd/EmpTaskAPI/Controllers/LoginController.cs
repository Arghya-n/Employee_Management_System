using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.HashPassword;
using EmpTaskAPI.Models;
using EmpTaskAPI.DTOModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
        public async Task<IActionResult> GetToken(UserLogin emp)

        {
            var employee = await _context.Employees.FirstOrDefaultAsync(e => e.Email == emp.Email);
            if (employee == null)
            {
                return BadRequest("Invalid Email");
            }

            var isValidPassword = PasswordHasher.HashPassword(emp.Password, employee.Salt) == employee.Password;
            if (!isValidPassword)
            {
                return Unauthorized("Invalid email or password");
            }

            // Generate access token
            string token = GenerateToken(employee);

            // Generate refresh token
            string refreshToken = GenerateRefreshToken();
            employee.RefreshToken = refreshToken;
            employee.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7); // Set refresh token validity for 7 days

            // Update the employee in the database
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Token = token,
                RefreshToken = refreshToken,
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
        
        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
            }
            return Convert.ToBase64String(randomNumber);
        }
        



    }
}
