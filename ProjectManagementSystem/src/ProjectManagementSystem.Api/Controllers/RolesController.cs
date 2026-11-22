using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementSystem.Application.Authorization;
using ProjectManagementSystem.Application.DTOs.Users;
using ProjectManagementSystem.Application.Interfaces;

namespace ProjectManagementSystem.Api.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IUserAdminService _users;

    public RolesController(IUserAdminService users)
    {
        _users = users;
    }

    [HttpGet]
    [Authorize(Policy = AppPermissions.Users.View)]
    public ActionResult<IReadOnlyList<RoleResponseDto>> GetAll()
    {
        return Ok(_users.GetRoles());
    }
}
