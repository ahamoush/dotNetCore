using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace ProjectManagementSystem.Data;

// يستخدمه dotnet ef فقط عند إنشاء الـ migrations، لأن الأداة تعمل خارج Program.cs.
public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    // IDesignTimeDbContextFactory :
    // هو مفتاح للأداة التي تسمح لـ dotnet ef بإنشاء السياق من ملف الإعدادات.
    // CreateDbContext :
    // هو الطريقة التي يستخدمها dotnet ef لإنشاء السياق.
    // string[] args :
    // هو المعاملات التي يتم تمريرها إلى الطريقة.
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        // ConfigurationBuilder :
        // هو الكلاس الذي يتم استخدامه لبناء البيانات التي يتم تمريرها إلى السياق.
        // SetBasePath :
        // هو الطريقة التي يستخدمها السياق لتحديد المسار الذي يتم استخدامه للبحث عن الملفات.
        // AddJsonFile :
        // هو الطريقة التي يستخدمها السياق لإضافة الملفات الى البيانات.
        // Build :
        // هو الطريقة التي يستخدمها السياق لبناء البيانات.

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .Build();
        // Configuration :
        // هو الكلاس الذي يتم استخدامه لبناء البيانات التي يتم تمريرها إلى السياق.
        // GetConnectionString :
        // هو الطريقة التي يستخدمها السياق للحصول على السلاسل القياسية المخصصة للاتصال بقاعدة البيانات.
        // DefaultConnection :
        // هو الاسم القياسي للسلاسل القياسية المخصصة للاتصال بقاعدة البيانات.
        // InvalidOperationException :
        // هو الكلاس الذي يتم استخدامه للتعامل مع الاستثناءات التي يتم تمريرها إلى الطريقة.

        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'DefaultConnection' was not found.");

        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new ApplicationDbContext(options);
    }
}
