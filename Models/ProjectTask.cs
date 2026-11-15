using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.Models;

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

    // Foreign Key
    public int ProjectId { get; set; }

    // Navigation Property
    public Project? Project { get; set; }
}