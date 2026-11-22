using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    public UsersController(IUserAdminService users)
    {
        _users = users;
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
        var actorId = User.FindFirst("sub")?.Value;
        if (string.IsNullOrEmpty(actorId))
            return Unauthorized();

        var result = await _users.SetRolesAsync(
            id,
            dto.Roles,
            actorId,
            User.IsInRole(AppRoles.SuperAdmin));

        return result.Status switch
        {
            UserAdminStatus.Ok => NoContent(),
            UserAdminStatus.NotFound => NotFound(new { message = result.Message }),
            UserAdminStatus.Forbidden => StatusCode(StatusCodes.Status403Forbidden, new { message = result.Message }),
            _ => BadRequest(new { message = result.Message })
        };
    }
}
