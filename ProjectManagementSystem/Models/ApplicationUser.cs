using Microsoft.AspNetCore.Identity;

namespace ProjectManagementSystem.Models;

// ApplicationUser = مستخدم النظام
// يرث من IdentityUser عشان ناخد: UserName, Email, PasswordHash, ...
// بدون ما نكتب نظام Users يدوي
public class ApplicationUser : IdentityUser
{
    // هل الحساب مفعّل؟ لو false منمنع الـ Login
    public bool IsActive { get; set; } = true;

    // تاريخ إنشاء الحساب (UTC)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
