namespace ProjectManagementSystem.DTOs.Projects;

// الشكل اللي بنرجعه للـ Client (مش Entity كامل)
public class ProjectResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public ProjectStatus Status { get; set; }

    public DateTime CreatedAt { get; set; }
}
