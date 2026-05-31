using IdentityApi.Models;
using IdentityApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly UserStore _store;
    private readonly TokenService _tokens;

    public AuthController(UserStore store, TokenService tokens)
    {
        _store = store;
        _tokens = tokens;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Email) || string.IsNullOrWhiteSpace(req.Password))
            return BadRequest(new { error = "email and password are required" });

        var user = _store.FindByEmail(req.Email);

        // Generic message — never reveal which field was wrong
        if (user == null || !PasswordHasher.Verify(req.Password, user.PasswordHash))
            return Unauthorized(new { error = "Invalid credentials" });

        var token = _tokens.GenerateToken(user);
        return Ok(new { token });
    }
}
