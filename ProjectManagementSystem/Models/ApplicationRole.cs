using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Models;

// ApplicationRole = دور المستخدم (SuperAdmin / Admin / User)
// يرث من IdentityRole عشان Identity يدير Roles و UserRoles و RoleClaims
public class ApplicationRole : IdentityRole
{
    public ApplicationRole()
    {
    }

    // Constructor مريح لإنشاء Role بالاسم
    public ApplicationRole(string roleName)
        : base(roleName)
    {
    }
}
