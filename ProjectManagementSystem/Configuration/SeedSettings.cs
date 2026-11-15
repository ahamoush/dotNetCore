namespace ProjectManagementSystem.Configuration;

// إعدادات إنشاء SuperAdmin عند بدء التطبيق
// القيم الحساسة (Password / Email) تيجي من User Secrets
public class SeedSettings
{
    public const string SectionName = "Seed";

    public string SuperAdminUserName { get; set; } = "abd";

    public string SuperAdminEmail { get; set; } = string.Empty;

    public string SuperAdminPassword { get; set; } = string.Empty;
}
