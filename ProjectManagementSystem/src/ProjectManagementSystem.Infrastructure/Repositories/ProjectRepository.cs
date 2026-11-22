using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.DTOs;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;
using ProjectManagementSystem.Infrastructure.Persistence;

namespace ProjectManagementSystem.Infrastructure.Repositories;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _context;

    public ProjectRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<Project>> ListAsync()
    {
        return _context.Projects.AsNoTracking().ToListAsync();
    }

    public Task<Project?> GetByIdAsync(int id, bool tracking)
    {
        var query = tracking
            ? _context.Projects
            : _context.Projects.AsNoTracking();

        return query.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task AddAsync(Project project)
    {
        await _context.Projects.AddAsync(project);
    }

    public Task<bool> AnyAsync(int id)
    {
        return _context.Projects.AnyAsync(p => p.Id == id);
    }

    public async Task<(List<Project> Items, int TotalCount)> SearchAsync(ProjectQueryDto query)
    {
        var projectsQuery = _context.Projects.AsNoTracking();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.Trim();
            projectsQuery = projectsQuery.Where(p =>
                p.Name.Contains(search) ||
                p.Description.Contains(search));
        }

        if (query.Status.HasValue)
            projectsQuery = projectsQuery.Where(p => p.Status == query.Status.Value);

        var sortBy = string.IsNullOrWhiteSpace(query.SortBy)
            ? "name"
            : query.SortBy.Trim().ToLowerInvariant();

        if (sortBy == "createdat")
        {
            projectsQuery = query.Descending
                ? projectsQuery.OrderByDescending(p => p.CreatedAt)
                : projectsQuery.OrderBy(p => p.CreatedAt);
        }
        else if (sortBy == "duedate")
        {
            projectsQuery = query.Descending
                ? projectsQuery.OrderByDescending(p => p.DueDate)
                : projectsQuery.OrderBy(p => p.DueDate);
        }
        else
        {
            projectsQuery = query.Descending
                ? projectsQuery.OrderByDescending(p => p.Name)
                : projectsQuery.OrderBy(p => p.Name);
        }

        var totalCount = await projectsQuery.CountAsync();
        var items = await projectsQuery
            .Skip((query.PageNumber - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        return (items, totalCount);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
