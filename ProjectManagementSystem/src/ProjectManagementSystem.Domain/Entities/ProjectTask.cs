namespace ProjectManagementSystem.Domain.Entities;

public class ProjectTask
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;

    public DateTime? DueDate { get; set; }

    public int ProjectId { get; set; }

    public Project? Project { get; set; }
}
