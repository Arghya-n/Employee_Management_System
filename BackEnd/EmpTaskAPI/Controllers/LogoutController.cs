using System.IdentityModel.Tokens.Jwt;
using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace EmpTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly AppDBContext context;
        private readonly ILogger<ProjectController> _logger;

        public LogoutController(AppDBContext context, ILogger<ProjectController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            Log.Information("Logout attempt with refresh token: {RefreshToken}", refreshToken);

            // Validate the refresh token
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);

            if (employee == null)
            {
                Log.Warning("Invalid logout attempt with refresh token: {RefreshToken}", refreshToken);
                return BadRequest("Invalid refresh token");
            }

            // Invalidate the refresh token by setting it to null
            employee.RefreshToken = null;
            employee.RefreshTokenExpiry = DateTime.MinValue; // Optionally reset the expiry date

            // Save changes to the database
            await context.SaveChangesAsync();

            Log.Information("Logout successful for employee ID: {EmployeeId}", employee.EmployeeId);
            return Ok("Logged out successfully");
        }
    }
}
