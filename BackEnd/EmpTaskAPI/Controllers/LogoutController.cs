using System.IdentityModel.Tokens.Jwt;
using EmpTaskAPI.DataAccessLayer;
using EmpTaskAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmpTaskAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogoutController : ControllerBase
    {
        private readonly AppDBContext context;

        public LogoutController(AppDBContext context)
        {
            this.context = context;
        }
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            // Validate the refresh token
            var employee = await context.Employees.FirstOrDefaultAsync(e => e.RefreshToken == refreshToken);

            if (employee == null)
            {
                return BadRequest("Invalid refresh token");
            }

            // Invalidate the refresh token by setting it to null
            employee.RefreshToken = null;
            employee.RefreshTokenExpiry = DateTime.MinValue; // Optionally reset the expiry date

            // Save changes to the database
            await context.SaveChangesAsync();

            return Ok("Logged out successfully");
        }

    }
}
