using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Infrastructure.Persistence;

public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}
