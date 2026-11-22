using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Application.Interfaces;
using ProjectManagementSystem.Domain.Entities;
using ProjectManagementSystem.Infrastructure.Persistence;

namespace ProjectManagementSystem.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _context;

    public TaskRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<List<ProjectTask>> ListAsync(int? projectId)
    {
        var query = _context.Tasks.AsNoTracking();

        if (projectId.HasValue)
            query = query.Where(t => t.ProjectId == projectId.Value);

        return query.ToListAsync();
    }

    public async Task<ProjectTask?> GetByIdAsync(int id, bool tracking)
    {
        if (tracking)
            return await _context.Tasks.FindAsync(id);

        return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task AddAsync(ProjectTask task)
    {
        await _context.Tasks.AddAsync(task);
    }

    public void Remove(ProjectTask task)
    {
        _context.Tasks.Remove(task);
    }

    public Task SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }
}
