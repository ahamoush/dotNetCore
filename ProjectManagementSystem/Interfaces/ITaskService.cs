using ProjectManagementSystem.DTOs.Tasks;

namespace ProjectManagementSystem.Interfaces;

// العقد الخاص بخدمات المهام
public interface ITaskService
{
    Task<IEnumerable<TaskResponseDto>> GetAllAsync(int? projectId);

    Task<TaskResponseDto?> GetByIdAsync(int id);

    Task<TaskResponseDto?> CreateAsync(CreateTaskDto dto);

    Task<bool> UpdateAsync(int id, UpdateTaskDto dto);

    Task<bool> DeleteAsync(int id);
}
