using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.DTOs.Tasks;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

// Service المهام: منطق العمل الخاص بالـ Tasks
public class TaskService : ITaskService
{
    private readonly ApplicationDbContext _context;

    public TaskService(ApplicationDbContext context)
    {
        _context = context;
    }

    // تحويل Entity إلى DTO
    private static TaskResponseDto ToResponseDto(ProjectTask task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            IsCompleted = task.IsCompleted,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId
        };
    }

    // جلب كل المهام، مع إمكانية الفلترة بـ projectId
    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(int? projectId)
    {
        var query = _context.Tasks.AsNoTracking();

        // لو المستخدم بعت projectId نفلتر المهام الخاصة بالمشروع ده فقط
        if (projectId.HasValue)
        {
            query = query.Where(t => t.ProjectId == projectId.Value);
        }

        var tasks = await query.ToListAsync();

        return tasks
            .Select(ToResponseDto)
            .ToList();
    }

    // جلب مهمة واحدة
    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (task is null)
            return null;

        return ToResponseDto(task);
    }

    // إنشاء مهمة جديدة
    // لازم المشروع يكون موجود قبل إضافة المهمة
    public async Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto)
    {
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == dto.ProjectId);

        // null معناها فشل الإنشاء بسبب مشروع غير موجود
        if (!projectExists)
            return null;

        var task = new ProjectTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return ToResponseDto(task);
    }

    // تعديل مهمة
    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        // FindAsync يبحث بالـ Primary Key
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
            return false;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.DueDate = dto.DueDate;

        await _context.SaveChangesAsync();

        return true;
    }

    // حذف نهائي للمهمة (Hard Delete)
    // ملاحظة: المشاريع Soft Delete، لكن المهام هنا بنحذفها فعلياً
    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _context.Tasks.FindAsync(id);

        if (task is null)
            return false;

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();

        return true;
    }
}
