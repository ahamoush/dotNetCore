using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Application.DTOs.Projects;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projects;

    public ProjectService(IProjectRepository projects)
    {
        _projects = projects;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    {
        var projects = await _projects.ListAsync();
        return projects.Select(ToResponseDto).ToList();
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _projects.GetByIdAsync(id, tracking: false);
        return project is null ? null : ToResponseDto(project);
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description
        };

        await _projects.AddAsync(project);
        await _projects.SaveChangesAsync();

        return ToResponseDto(project);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await _projects.GetByIdAsync(id, tracking: true);
        if (project is null)
            return false;

        project.Name = dto.Name;
        project.Description = dto.Description;
        project.UpdatedAt = DateTime.UtcNow;

        await _projects.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _projects.GetByIdAsync(id, tracking: true);
        if (project is null)
            return false;

        project.IsDeleted = true;
        project.DeletedAt = DateTime.UtcNow;

        await _projects.SaveChangesAsync();
        return true;
    }

    public async Task<PagedResultDto<ProjectResponseDto>> GetProjectsAsync(ProjectQueryDto query)
    {
        var (items, totalCount) = await _projects.SearchAsync(query);

        return new PagedResultDto<ProjectResponseDto>
        {
            Items = items.Select(ToResponseDto).ToList(),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }

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
}
