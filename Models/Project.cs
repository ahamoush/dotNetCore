namespace ProjectManagementSystem.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; } = ProjectStatus.New;

    public DateTime StartDate { get; set; }

    public DateTime? DueDate { get; set; }

    public ICollection<ProjectTask> Tasks { get; set; }
        = new List<ProjectTask>();
}