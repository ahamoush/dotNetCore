using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.DTOs.Tasks;

public class CreateTaskDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public DateTime? DueDate { get; set; }

    [Range(1, int.MaxValue)]
    public int ProjectId { get; set; }
}