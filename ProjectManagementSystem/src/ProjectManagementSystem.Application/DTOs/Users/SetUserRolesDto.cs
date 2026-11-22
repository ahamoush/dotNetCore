using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Application.DTOs.Users;

public class SetUserRolesDto
{
    [Required]
    [MinLength(1)]
    public List<string> Roles { get; set; } = [];
}
