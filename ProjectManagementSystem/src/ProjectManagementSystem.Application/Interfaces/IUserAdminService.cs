using ProjectManagementSystem.Application.DTOs.Users;

namespace ProjectManagementSystem.Application.Interfaces;

public interface IUserAdminService
{
    Task<IReadOnlyList<UserResponseDto>> GetUsersAsync();

    IReadOnlyList<RoleResponseDto> GetRoles();

    Task<UserAdminResult> SetRolesAsync(
        string userId,
        IReadOnlyList<string> roles,
        string actorUserId,
        bool actorIsSuperAdmin);
}
