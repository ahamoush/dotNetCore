using System.ComponentModel.DataAnnotations;

namespace ProjectManagementSystem.DTOs.Projects;

// البيانات المطلوبة لتعديل مشروع موجود
public class UpdateProjectDto
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;
}
