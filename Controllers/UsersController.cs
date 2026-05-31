using IdentityApi.Models;
using IdentityApi.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly UserStore _store;
    public UsersController(UserStore store) => _store = store;

    [HttpPost("register")]
    public IActionResult Register([FromBody] RegisterRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Username) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { error = "username, email, and password are required" });

        if (req.Password.Length < 6)
            return BadRequest(new { error = "Password must be at least 6 characters" });

        var user = new User
        {
            Username = req.Username,
            Email = req.Email,
            PasswordHash = PasswordHasher.Hash(req.Password),
        };

        if (!_store.TryAdd(user))
            return Conflict(new { error = "Email already registered" });

        return StatusCode(201, new { id = user.Id, username = user.Username, email = user.Email });
    }

    [HttpGet("me")]
    public IActionResult Me()
    {
        if (HttpContext.User.Identity?.IsAuthenticated != true)
            return Unauthorized(new { error = "Authentication required" });

        // JwtSecurityTokenHandler maps "email" claim -> ClaimTypes.Email (xmlsoap URI)
        var email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

        if (string.IsNullOrEmpty(email))
            return Unauthorized(new { error = "Invalid token — email claim missing" });

        var user = _store.FindByEmail(email);
        if (user == null) return NotFound(new { error = "User not found" });

        return Ok(new { id = user.Id, username = user.Username, email = user.Email, roles = user.Roles });
    }
}
