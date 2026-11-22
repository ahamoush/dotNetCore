using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Interfaces;

public interface IProjectRepository
{
    Task<List<Project>> ListAsync();

    Task<Project?> GetByIdAsync(int id, bool tracking);

    Task AddAsync(Project project);

    Task<bool> AnyAsync(int id);

    Task<(List<Project> Items, int TotalCount)> SearchAsync(ProjectQueryDto query);

    Task SaveChangesAsync();
}
