using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.DTOs.Tasks;

// بيانات تعديل مهمة
public class UpdateTaskDto
{
    [Required]
    [StringLength(150, MinimumLength = 3)]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }

    public DateTime? DueDate { get; set; }
}
