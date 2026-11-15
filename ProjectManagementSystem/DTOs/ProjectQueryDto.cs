// معاملات البحث والترقيم اللي بتيجي من Query String
// مثال: /api/projects?search=api&status=1&pageNumber=1&pageSize=10
public class ProjectQueryDto
{
    public string? Search { get; set; }

    public ProjectStatus? Status { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    // name | createdat | duedate
    public string SortBy { get; set; } = "Name";

    // true = من الأكبر للأصغر
    public bool Descending { get; set; } = false;
}
