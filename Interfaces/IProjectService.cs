using ProjectManagementSystem.DTOs.Projects;

namespace ProjectManagementSystem.Interfaces;

public interface IProjectService
{
    Task<IEnumerable<ProjectResponseDto>> GetAllAsync();

    Task<ProjectResponseDto?> GetByIdAsync(int id);

    Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto);

    Task<bool> UpdateAsync(int id, UpdateProjectDto dto);

    Task<bool> DeleteAsync(int id);

    Task<PagedResultDto<ProjectResponseDto>> GetProjectsAsync(ProjectQueryDto query);
}