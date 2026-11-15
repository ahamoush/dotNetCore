using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models;

// جدول المهام
public class ProjectTask
{
    public int Id { get; set; }

    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; } = false;

    public DateTime? DueDate { get; set; }

    // Foreign Key: يربط المهمة بمشروع معين
    public int ProjectId { get; set; }

    // Navigation Property: للوصول لكائن المشروع المرتبط
    public Project? Project { get; set; }
}
