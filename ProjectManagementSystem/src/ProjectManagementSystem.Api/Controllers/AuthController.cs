using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Validation;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Auth;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IValidator<RegisterDto> _registerValidator;
    private readonly IValidator<LoginDto> _loginValidator;

    public AuthController(
        IAuthService auth,
        IValidator<RegisterDto> registerValidator,
        IValidator<LoginDto> loginValidator)
    {
        _auth = auth;
        _registerValidator = registerValidator;
        _loginValidator = loginValidator;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _registerValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        AuthResult result = await _auth.RegisterAsync(dto);
        if (!result.Succeeded)
            return BadRequest(new { errors = result.Errors });

        return Ok(result.Data);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _loginValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        AuthResult result = await _auth.LoginAsync(dto);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid email or password." });

        return Ok(result.Data);
    }

    [Authorize]
    [HttpGet("me")]
    public ActionResult<CurrentUserDto> Me()
    {
        var roles = new List<string>();
        foreach (var claim in User.FindAll(AppClaimTypes.Role))
        {
            if (!roles.Contains(claim.Value))
                roles.Add(claim.Value);
        }

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
