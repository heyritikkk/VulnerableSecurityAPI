namespace VulnerableSecurityAPI.Controllers;

using Microsoft.AspNetCore.Mvc;
using VulnerableSecurityAPI.DTOs;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    // TODO: This is a stub for future security demonstrations.
    // Real JWT generation and proper authentication will be added,
    // and intentionally weakened for SAST testing in later stages.

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        // For now, this is a mock login endpoint.
        if (loginDto.Username == "admin" && loginDto.Password == "adminpassword123")
        {
            return Ok(new { Message = "Login successful (Mock)", Token = "mock-jwt-token-for-later" });
        }

        return Unauthorized(new { Message = "Invalid credentials" });
    }
}
