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



        /// <summary>
        /// Logs out the user by invalidating the refresh token.
        /// </summary>
        /// <param name="refreshToken">The refresh token to invalidate.</param>
        /// <returns>An IActionResult indicating the result of the logout operation.</returns>
        /// <response code="200">Successfully logged out and refreshed token invalidated.</response>
        /// <response code="400">Invalid refresh token provided.</response>
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
