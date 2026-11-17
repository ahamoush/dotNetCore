using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Models;

// مستخدم النظام.
// IdentityUser يعطينا Id و Email و UserName و PasswordHash جاهزة.
public class ApplicationUser : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
}