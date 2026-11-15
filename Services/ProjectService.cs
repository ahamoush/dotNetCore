using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Data;
using ProjectManagementSystem.DTOs.Projects;
using ProjectManagementSystem.Interfaces;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Services;

public class ProjectService : IProjectService
{
    private static readonly Expression<Func<Project, ProjectResponseDto>> ToResponse =
        p => new ProjectResponseDto
        {
            Id = p.Id,
            Name = p.Name,
            Description = p.Description,
            Status = p.Status,
            CreatedAt = p.CreatedAt
        };

    private readonly ApplicationDbContext _context;

    public ProjectService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllAsync()
    {
        return await _context.Projects
            .AsNoTracking()
            .Select(ToResponse)
            .ToListAsync();
    }

    public async Task<ProjectResponseDto?> GetByIdAsync(int id)
    {
        return await _context.Projects
            .AsNoTracking()
            .Where(p => p.Id == id)
            .Select(ToResponse)
            .FirstOrDefaultAsync();
    }

    public async Task<ProjectResponseDto> CreateAsync(CreateProjectDto dto)
    {
        var project = new Project
        {
            Name = dto.Name,
            Description = dto.Description
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Status = project.Status,
            CreatedAt = project.CreatedAt
        };
    }

    public async Task<bool> UpdateAsync(int id, UpdateProjectDto dto)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return false;

        project.Name = dto.Name;
        project.Description = dto.Description;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == id);

        if (project is null)
            return false;

        project.IsDeleted = true;
        project.DeletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<PagedResultDto<ProjectResponseDto>> GetProjectsAsync(ProjectQueryDto query)
    {
        var projects = _context.Projects.AsNoTracking();

        // Searching
        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            projects = projects.Where(p =>
                p.Name.Contains(search) ||
                p.Description.Contains(search));
        }

        // Filtering
        if (query.Status.HasValue)
        {
            var status = query.Status.Value;
            projects = projects.Where(p => p.Status == status);
        }

        // Sorting
        var sortBy = string.IsNullOrWhiteSpace(query.SortBy)
            ? "name"
            : query.SortBy.ToLowerInvariant();

        projects = sortBy switch
        {
            "createdat" => query.Descending
                ? projects.OrderByDescending(p => p.CreatedAt)
                : projects.OrderBy(p => p.CreatedAt),

            "duedate" => query.Descending
                ? projects.OrderByDescending(p => p.DueDate)
                : projects.OrderBy(p => p.DueDate),

            _ => query.Descending
                ? projects.OrderByDescending(p => p.Name)
                : projects.OrderBy(p => p.Name)
        };

        var totalCount = await projects.CountAsync();

        // Pagination
        var items = await projects
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .Select(ToResponse)
            .ToListAsync();

        return new PagedResultDto<ProjectResponseDto>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = query.PageNumber,
            PageSize = query.PageSize
        };
    }
}
