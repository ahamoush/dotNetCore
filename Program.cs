using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Services;
using ProjectManagementSystem.Middleware;

// تقوم بإنشاء البيئة الذي يحتوي على الخدمات
var builder = WebApplication.CreateBuilder(args);

// Controllers
// تقوم بإضافة المستكشفين إلى المحتوى
builder.Services.AddControllers();

// Database
// تقوم بإضافة القاعدة البيانات إلى المحتوى
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// Dependency Injection
// تقوم بإضافة الخدمات إلى المحتوى
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();

// Swagger
// تقوم بإضافة المستكشفين إلى المحتوى
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// تقوم ببناء التطبيق الذي يحتوي على الخدمات
var app = builder.Build();
app.UseMiddleware<ExceptionHandlingMiddleware>();

// HTTP Pipeline
// تقوم بتنفيذ الإذنات
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// تقوم بتنفيذ الإذنات
app.UseHttpsRedirection();

// تقوم بتنفيذ الإذنات
app.UseAuthorization();

// تقوم بتنفيذ المسارات
app.MapControllers();

// تقوم بتنفيذ التطبيق
app.Run();