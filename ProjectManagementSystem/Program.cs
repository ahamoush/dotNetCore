using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ProjectManagementSystem.Configuration;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Middleware;
using ProjectManagementSystem.Models;
using ProjectManagementSystem.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// قراءة إعدادات JWT و Seed من Configuration
builder.Services.Configure<JwtSettings>(
    builder.Configuration.GetSection(JwtSettings.SectionName));

builder.Services.Configure<SeedSettings>(
    builder.Configuration.GetSection(SeedSettings.SectionName));

var jwtSettings = builder.Configuration
    .GetSection(JwtSettings.SectionName)
    .Get<JwtSettings>() ?? new JwtSettings();

// التأكد إن مفتاح التوقيع موجود وقوي بما يكفي
if (string.IsNullOrWhiteSpace(jwtSettings.Key) || jwtSettings.Key.Length < 32)
{
    throw new InvalidOperationException(
        "Jwt:Key is missing or too short. " +
        "Set a key with at least 32 characters in User Secrets (dotnet user-secrets set \"Jwt:Key\" \"YOUR_LONG_SECRET_KEY\").");
}

// EF Core + SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// ASP.NET Core Identity
builder.Services
    .AddIdentity<ApplicationUser, ApplicationRole>(options =>
    {
        // إعدادات كلمة المرور (مناسبة للتعلم + أمان مقبول)
        options.Password.RequiredLength = 6;
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireUppercase = false;
        options.Password.RequireNonAlphanumeric = false;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// JWT Authentication
builder.Services
    .AddAuthentication(options =>
    {
        // الافتراضي: Bearer JWT (مش Cookie)
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSettings.Key)),
            // لو التوكن خلص صلاحيته يتم رفضه فوراً
            ClockSkew = TimeSpan.Zero
        };
    });

// Authorization Policies (Claim-Based)
builder.Services.AddAuthorization(options =>
{
    // Role = مجموعة كبيرة (Admin)
    // Claim = صلاحية دقيقة (users.read)
    // Policy = قاعدة نسميها ونستخدمها في [Authorize(Policy = "...")]
    options.AddPolicy(AppPolicies.CanReadUsers, policy =>
        policy.RequireClaim(AppClaimTypes.Permission, AppPermissions.UsersRead));

    options.AddPolicy(AppPolicies.CanCreateUsers, policy =>
        policy.RequireClaim(AppClaimTypes.Permission, AppPermissions.UsersCreate));

    options.AddPolicy(AppPolicies.CanManageUsers, policy =>
        policy.RequireClaim(AppClaimTypes.Permission, AppPermissions.UsersDelete));
});

// Services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IdentitySeedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Project Management API",
        Version = "v1"
    });

    // زر Authorize في Swagger لتجربة JWT بسهولة
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Paste your JWT token here. Example: eyJhbGciOi..."
    });

    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
    });
});

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// الترتيب مهم:
// Authentication أولاً (يقرأ التوكن)
// ثم Authorization (يقرر هل مسموح ولا لأ)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// تطبيق Migrations + Seeding عند بدء التشغيل
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        var db = services.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();

        var seeder = services.GetRequiredService<IdentitySeedService>();
        await seeder.SeedAsync();
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Database migration or identity seeding failed.");
        throw;
    }
}

app.Run();
