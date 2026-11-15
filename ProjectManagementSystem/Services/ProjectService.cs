using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.DTOs.Projects;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

// Service المشاريع: هنا منطق العمل (Business Logic) المتعلق بالمشاريع
// Controller يستدعي هذا الكلاس، وهذا الكلاس يتعامل مع قاعدة البيانات عبر DbContext
public class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _context;

    // Dependency Injection: ASP.NET يمرر ApplicationDbContext تلقائياً
    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    // تحويل Entity (Project) إلى DTO (ProjectResponseDto)
    // لماذا؟ عشان ما نرجع جدول الداتابيز مباشرة للـ API
    private static ProjectResponseDto ToResponseDto(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status,
            CreatedAt = project.CreatedAt
        };
    }

    // جلب كل المشاريع
    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    {
        // AsNoTracking = قراءة فقط (Read-only)
        // مفيد لما مش هنعدّل البيانات، وبيخلي الاستعلام أخف
        var projects = await _context.Projects
            .AsNoTracking()
            .ToListAsync();

        // بعد ما البيانات جت من الداتابيز، نحولها إلى DTOs بطريقة بسيطة
        var result = projects
            .Select(ToResponseDto)
            .ToList();

        return result;
    }

    // جلب مشروع واحد بالـ Id
    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _context.Projects
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);

        // لو مش موجود نرجع null، والـ Controller يرجع 404
        if (project is null)
            return null;

        return ToResponseDto(project);
    }

    // إنشاء مشروع جديد
    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description
        };

        // Add = وضع الصف في حالة Added داخل Change Tracker
        _context.Projects.Add(project);

        // SaveChangesAsync = تنفيذ INSERT فعلياً في SQL Server
        // CreatedAt يتم تعبئته تلقائياً داخل ApplicationDbContext.SaveChangesAsync
        await _context.SaveChangesAsync();

        return ToResponseDto(project);
    }

    // تعديل مشروع موجود
    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return false;

        project.Name = dto.Name;
        project.Description = dto.Description;

        // EF Core يتتبع التغييرات ويعمل UPDATE عند SaveChanges
        await _context.SaveChangesAsync();

        return true;
    }

    // Soft Delete:
    // لا نحذف الصف من الداتابيز، بل نعلّمه كمحذوف (IsDeleted = true)
    // Query Filter في DbContext يخفيه تلقائياً من نتائج البحث العادية
    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return false;

        project.IsDeleted = true;
        project.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    // بحث + فلترة + ترتيب + ترقيم صفحات (Pagination)
    public async Task<PagedResultDto<ProjectResponseDto>> GetProjectsAsync(ProjectQueryDto query)
    {
        // نبدأ من جدول Projects (IQueryable = الاستعلام لسه ما اتنفذش)
        var projectsQuery = _context.Projects.AsNoTracking();

        // 1) البحث بالاسم أو الوصف
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();

            projectsQuery = projectsQuery.Where(p =>
                p.Name.Contains(search) ||
                p.Description.Contains(search));
        }

        // 2) الفلترة حسب الحالة (Status)
        if (query.Status.HasValue)
        {
            projectsQuery = projectsQuery.Where(p => p.Status == query.Status.Value);
        }

        // 3) الترتيب (Sorting) - كتبناه بـ if/else عشان يكون أوضح للمبتدئ
        var sortBy = string.IsNullOrWhiteSpace(query.SortBy)
            ? "name"
            : query.SortBy.Trim().ToLowerInvariant();

        if (sortBy == "createdat")
        {
            if (query.Descending)
                projectsQuery = projectsQuery.OrderByDescending(p => p.CreatedAt);
            else
                projectsQuery = projectsQuery.OrderBy(p => p.CreatedAt);
        }
        else if (sortBy == "duedate")
        {
            if (query.Descending)
                projectsQuery = projectsQuery.OrderByDescending(p => p.DueDate);
            else
                projectsQuery = projectsQuery.OrderBy(p => p.DueDate);
        }
        else
        {
            // الافتراضي: الترتيب بالاسم
            if (query.Descending)
                projectsQuery = projectsQuery.OrderByDescending(p => p.Name);
            else
                projectsQuery = projectsQuery.OrderBy(p => p.Name);
        }

        // عدد النتائج الكلي قبل الترقيم (مهم لحساب عدد الصفحات)
        var totalCount = await projectsQuery.CountAsync();

        // 4) Pagination:
        // Page 1 => Skip(0)
        // Page 2 => Skip(PageSize)
        // Page 3 => Skip(PageSize * 2) ...
        var projects = await projectsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var items = projects
            .Select(ToResponseDto)
            .ToList();

        return new PagedResultDto<ProjectResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
