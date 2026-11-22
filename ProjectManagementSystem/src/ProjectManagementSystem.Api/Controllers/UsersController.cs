using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Api.Validation;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Users;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserAdminService _users;
    private readonly IValidator<SetUserRolesDto> _rolesValidator;

    public UsersController(IUserAdminService users, IValidator<SetUserRolesDto> rolesValidator)
    {
        _users = users;
        _rolesValidator = rolesValidator;
    }

    [HttpGet]
    [Authorize(Policy = AppPermissions.Users.View)]
    public async Task<ActionResult<IReadOnlyList<UserResponseDto>>> GetAll()
    {
        return Ok(await _users.GetUsersAsync());
    }

    [HttpPut("{id}/roles")]
    [Authorize(Policy = AppPermissions.Users.ManageRoles)]
    public async Task<IActionResult> SetRoles(string id, SetUserRolesDto dto)
    {
        IActionResult? invalid = await InputValidation.CheckAsync(
            _rolesValidator,
            dto,
            HttpContext.RequestAborted);
        if (invalid is not null)
            return invalid;

        string? actorId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(actorId))
            return Unauthorized();

        UserAdminResult result = await _users.SetRolesAsync(
            id,
            dto.Roles,
            actorId,
            User.IsInRole(AppRoles.SuperAdmin));

        if (result.Status == UserAdminStatus.Ok)
            return NoContent();

        if (result.Status == UserAdminStatus.NotFound)
            return NotFound(new { message = result.Message });

        if (result.Status == UserAdminStatus.Forbidden)
            return StatusCode(StatusCodes.Status403Forbidden, new { message = result.Message });

        return BadRequest(new { message = result.Message });
    }
}
