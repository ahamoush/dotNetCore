using ProjectManagementSystem.Application.DTOs.Tasks;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IProjectRepository _projects;

    public TaskService(ITaskRepository tasks, IProjectRepository projects)
    {
        _tasks = tasks;
        _projects = projects;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(int? projectId)
    {
        var tasks = await _tasks.ListAsync(projectId);
        return tasks.Select(ToResponseDto).ToList();
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _tasks.GetByIdAsync(id, tracking: false);
        return task is null ? null : ToResponseDto(task);
    }

    public async Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto)
    {
        if (!await _projects.AnyAsync(dto.ProjectId))
            return null;

        var task = new ProjectTask
        {
            Title = dto.Title,
            Description = dto.Description,
            DueDate = dto.DueDate,
            ProjectId = dto.ProjectId
        };

        await _tasks.AddAsync(task);
        await _tasks.SaveChangesAsync();

        return ToResponseDto(task);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _tasks.GetByIdAsync(id, tracking: true);
        if (task is null)
            return false;

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;
        task.DueDate = dto.DueDate;

        await _tasks.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var task = await _tasks.GetByIdAsync(id, tracking: true);
        if (task is null)
            return false;

        _tasks.Remove(task);
        await _tasks.SaveChangesAsync();
        return true;
    }

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
}
