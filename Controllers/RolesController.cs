using IdentityApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApi.Controllers;

[ApiController]
[Route("api/roles")]
public class RolesController : ControllerBase
{
    private readonly RoleStore _roles;
    public RolesController(RoleStore roles) => _roles = roles;

    [HttpGet]
    public IActionResult GetRoles() => Ok(new { roles = _roles.GetAll() });
}
