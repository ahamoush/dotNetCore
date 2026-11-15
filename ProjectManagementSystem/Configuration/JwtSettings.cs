namespace ProjectManagementSystem.Configuration;

// إعدادات JWT من appsettings.json + User Secrets
public class JwtSettings
{
    // اسم القسم داخل ملف الإعدادات
    public const string SectionName = "Jwt";

    // مين اللي أصدر التوكن (API بتاعتنا)
    public string Issuer { get; set; } = string.Empty;

    // مين المفروض يستخدم التوكن (Client)
    public string Audience { get; set; } = string.Empty;

    // مدة صلاحية التوكن بالدقائق
    public int ExpirationInMinutes { get; set; } = 60;

    // مفتاح التوقيع السري (يُخزَّن في User Secrets مش في Git)
    public string Key { get; set; } = string.Empty;
}
