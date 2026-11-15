using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Middleware;

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

// Dependency Injection:
// عندما يطلب Controller واجهة IProjectService، ASP.NET يعطيه كائن ProjectService
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Swagger: واجهة تجريبية للـ API (مفيدة جداً أثناء التعلم والاختبار)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
