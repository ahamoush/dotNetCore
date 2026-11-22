using AutoMapper;
using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Application.DTOs.Projects;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;

namespace ProjectManagementSystem.Application.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projects;
    private readonly IMapper _mapper;

    public ProjectService(IProjectRepository projects, IMapper mapper)
    {
        _projects = projects;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    {
        var projects = await _projects.ListAsync();
        return _mapper.Map<List<ProjectResponseDto>>(projects);
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        var project = await _projects.GetByIdAsync(id, tracking: false);
        return project is null ? null : _mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    {
        var project = _mapper.Map<Project>(dto);

        await _projects.AddAsync(project);
        await _projects.SaveChangesAsync();

        return _mapper.Map<ProjectResponseDto>(project);
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await _projects.GetByIdAsync(id, tracking: true);
        if (project is null)
            return false;

        _mapper.Map(dto, project);
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
            Items = _mapper.Map<List<ProjectResponseDto>>(items),
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
