using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Interfaces;

public interface ITaskRepository
{
    Task<List<ProjectTask>> ListAsync(int? projectId);

    Task<ProjectTask?> GetByIdAsync(int id, bool tracking);

    Task AddAsync(ProjectTask task);

    void Remove(ProjectTask task);

    Task SaveChangesAsync();
}
