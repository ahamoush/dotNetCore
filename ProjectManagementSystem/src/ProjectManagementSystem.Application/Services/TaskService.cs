using AutoMapper;
using ProjectManagementSystem.Application.DTOs.Tasks;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public TaskService(ITaskRepository tasks, IProjectRepository projects, IMapper mapper)
    {
        _tasks = tasks;
        _projects = projects;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TaskResponseDto>> GetAllAsync(int? projectId)
    {
        var tasks = await _tasks.ListAsync(projectId);
        return _mapper.Map<List<TaskResponseDto>>(tasks);
    }

    public async Task<TaskResponseDto?> GetByIdAsync(int id)
    {
        var task = await _tasks.GetByIdAsync(id, tracking: false);
        return task is null ? null : _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto)
    {
        if (!await _projects.AnyAsync(dto.ProjectId))
            return null;

        var task = _mapper.Map<ProjectTask>(dto);

        await _tasks.AddAsync(task);
        await _tasks.SaveChangesAsync();

        return _mapper.Map<TaskResponseDto>(task);
    }

    public async Task<bool> UpdateAsync(int id, UpdateTaskDto dto)
    {
        var task = await _tasks.GetByIdAsync(id, tracking: true);
        if (task is null)
            return false;

        _mapper.Map(dto, task);

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
}
