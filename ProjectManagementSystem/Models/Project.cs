namespace ProjectManagementSystem.Models;

// جدول المشاريع
public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    // حالة المشروع (New / InProgress / ...)
    public ProjectStatus Status { get; set; } = ProjectStatus.New;

    public DateTime StartDate { get; set; }

    // ? معناها القيمة ممكن تكون null
    public DateTime? DueDate { get; set; }

    // Navigation Property:
    // علاقة One-to-Many مع المهام
    public ICollection<ProjectTask> Tasks { get; set; }
        = new List<ProjectTask>();
}
