using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Middleware;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

// CreateBuilder: يبدأ تجهيز التطبيق ويقرأ الإعدادات (مثل appsettings.json)
var builder = WebApplication.CreateBuilder(args);

// AddControllers: يفعّل Controllers عشان نقدر نكتب API Endpoints
builder.Services.AddControllers();

// ربط EF Core مع SQL Server باستخدام Connection String من appsettings.json
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Token providers تشفّر رموز تأكيد الإيميل وإعادة كلمة السر.
// التسجيل الصريح يمنع فشل dotnet ef لأنه لا يجد IDataProtectionProvider.
builder.Services.AddDataProtection();

// مستخدمون وأدوار. RequireUniqueEmail يمنع تكرار الإيميل.
builder.Services
    .AddIdentityCore<ApplicationUser>(options =>
    {
        options.User.RequireUniqueEmail = true;

        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
    })
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

// Dependency Injection:
// عندما يطلب Controller واجهة IProjectService، ASP.NET يعطيه كائن ProjectService
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Swagger: واجهة تجريبية للـ API (مفيدة جداً أثناء التعلم والاختبار)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// يملأ AspNetRoles بالأدوار الستة إذا كانت ناقصة.
// شغّل الـ Migration قبل أول تشغيل، وإلا يفشل التشغيل لأن الجداول غير موجودة.
using (var scope = app.Services.CreateScope())
{
    await IdentitySeeder.SeedRolesAsync(scope.ServiceProvider);
}

// Middleware عام للأخطاء: لو حصل Exception غير متوقع يرجع JSON واضح بدل صفحة خطأ
app.UseMiddleware<ExceptionHandlingMiddleware>();

// في وضع التطوير فقط نفتح Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// يربط مسارات الـ Controllers (مثل /api/projects)
app.MapControllers();

app.Run();