using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Auth;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth)
    {
        _auth = auth;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto dto)
    {
        var result = await _auth.RegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto dto)
    {
        var result = await _auth.LoginAsync(dto);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserDto> Me()
    {
        var roles = User.FindAll(AppClaimTypes.Role)
            .Select(claim => claim.Value)
            .Distinct(StringComparer.Ordinal)
            .ToList();

        return Ok(new CurrentUserDto
        {
            Id = User.FindFirst("sub")?.Value ?? string.Empty,
            Email = User.FindFirst("email")?.Value ?? string.Empty,
            FullName = User.FindFirst(AppClaimTypes.Name)?.Value ?? string.Empty,
            Roles = roles,
            Permissions = RolePermissionMap.ForRoles(roles).ToList()
        });
    }
}
