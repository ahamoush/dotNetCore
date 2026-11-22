using ProjectManagementSystem.Application.DTOs.Tasks;

namespace ProjectManagementSystem.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllAsync(int? projectId);

    Task<TaskResponseDto?> GetByIdAsync(int id);

    Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto);

    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);
}
