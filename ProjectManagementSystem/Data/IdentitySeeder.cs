using Microsoft.AspNetCore.Identity;
// Microsoft.AspNetCore.Identity يجلب RoleManager وIdentityRole.
using ProjectManagementSystem.Models;
// ProjectManagementSystem.Models يجلب AppRoles، وهو مصدر أسماء الأدوار.
namespace ProjectManagementSystem.Data;

// IdentitySeeder = يسمح لنا بإنشاء أدوار المستخدمين في قاعدة البيانات.
public static class IdentitySeeder
{
    // SeedRolesAsync = يسمح لنا بإنشاء أدوار المستخدمين في قاعدة البيانات.
    public static async Task SeedRolesAsync(IServiceProvider services)
    {   // RoleManager = يسمح لنا بإنشاء أدوار المستخدمين في قاعدة البيانات.
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        // AppRoles.All = يجلب جميع أسماء الأدوار من AppRoles.
        foreach (var roleName in AppRoles.All)
        {
            // RoleExistsAsync = يتحقق من وجود الأدار في قاعدة البيانات.
            if (await roleManager.RoleExistsAsync(roleName))
                continue; // لو الأدار موجود في قاعدة البيانات ينتقل للدور التالي.

            // CreateAsync = ينشئ الأدار في قاعدة البيانات.
            var result = await roleManager.CreateAsync(new IdentityRole(roleName));
            // Succeeded = يتحقق من نجاح إنشاء الأدار.      لو لم ينجح يطلع الخطأ.

            if (!result.Succeeded) // لو لم ينجح يطلع الخطأ.
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description)); // يطلع الخطأ.
                throw new InvalidOperationException(
                    $"Failed to create role '{roleName}': {errors}"); // يطلع الخطأ.
                // InvalidOperationException = يطلع الخطأ.
            }
        }
    }
}