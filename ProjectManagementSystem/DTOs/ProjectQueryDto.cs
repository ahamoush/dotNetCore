public class ProjectQueryDto
{
    public string? Search { get; set; }

    public ProjectStatus? Status { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string SortBy { get; set; } = "Name";

    public bool Descending { get; set; } = false;
}