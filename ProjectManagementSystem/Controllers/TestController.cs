using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Configuration;

namespace ProjectManagementSystem.Controllers;

// Endpoints للتجربة فقط (مش Business Logic)
[ApiController]
[Route("api/test")]
public class TestController : ControllerBase
{
    // متاح للجميع بدون Login
    [AllowAnonymous]
    [HttpGet("public")]
    public IActionResult Public()
    {
        return Ok(new { message = "This endpoint is public. No token needed." });
    }

    // أي مستخدم مسجّل دخول (مع JWT صالح)
    [Authorize]
    [HttpGet("authenticated")]
    public IActionResult Authenticated()
    {
        return Ok(new
        {
            message = "You are authenticated.",
            userName = User.Identity?.Name
        });
    }

    // Admin أو SuperAdmin فقط
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.SuperAdmin}")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new { message = "Admin or SuperAdmin access granted." });
    }

    // SuperAdmin فقط
    [Authorize(Roles = AppRoles.SuperAdmin)]
    [HttpGet("super-admin")]
    public IActionResult SuperAdminOnly()
    {
        return Ok(new { message = "SuperAdmin access granted." });
    }

    // Policy-Based: يحتاج Claim permission = users.delete
    [Authorize(Policy = AppPolicies.CanManageUsers)]
    [HttpGet("manage-users")]
    public IActionResult ManageUsers()
    {
        return Ok(new
        {
            message = "CanManageUsers policy access granted.",
            hint = "This policy checks the 'permission' claim value 'users.delete'."
        });
    }
}
