
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Middleware;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

// new...
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

// CreateBuilder: يبدأ تجهيز التطبيق ويقرأ الإعدادات (مثل appsettings.json)
var builder = WebApplication.CreateBuilder(args);

// AddControllers: يفعّل Controllers عشان نقدر نكتب API Endpoints
builder.Services.AddControllers();

// ربط EF Core مع SQL Server باستخدام Connection String من appsettings.json
builder.Services. <ApplicationDbContext>(options =>
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
//...new : عندما يطلب AuthController واجهة IAuthService، ASP.NET يعطيه AuthService
builder.Services.AddScoped<IAuthService, AuthService>();

//...new : مفتاح توقيع التوكن من appsettings.Development.json. غيابه يوقف التشغيل فوراً.
var jwtKey = builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException("Jwt:Key is missing.");

//...new : JWT هو طريقة التحقق. العميل يرسل التوكن في ترويسة Authorization.
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        //...new : نبقي أسماء الـ claims كما كُتبت في التوكن، من غير إعادة تسمية تلقائية.
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //...new : يرفض التوكن لو المصدر أو الجمهور أو مدة الصلاحية أو التوقيع غير مطابقة.
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            //...new : [Authorize(Roles = "...")] يقرأ الدور من هذا الـ claim.
            RoleClaimType = ClaimTypes.Role,
            NameClaimType = ClaimTypes.Name
        };
    });

// Swagger: واجهة تجريبية للـ API (مفيدة جداً أثناء التعلم والاختبار)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    //...new : يعرّف زر Authorize في Swagger حتى نلصق التوكن أثناء التجربة.
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Description = "الصق التوكن فقط، من غير كلمة Bearer."
    });

    //...new : يطبق هذا التعريف على المسارات، فيرسل Swagger الترويسة مع كل طلب.
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("bearer", document)] = []
    });
});

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
//...new : يقرأ التوكن ويبني هوية المستخدم. لازم يسبق UseAuthorization.
app.UseAuthentication();
app.UseAuthorization();

// يربط مسارات الـ Controllers (مثل /api/projects)
app.MapControllers();

app.Run();
