// شكل الاستجابة لما نستخدم Pagination
public class PagedResultDto<T>
{
    // عناصر الصفحة الحالية
    public IEnumerable<T> Items { get; set; } = [];

    // العدد الكلي لكل النتائج (مش بس الصفحة الحالية)
    public int TotalCount { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    // يحسب عدد الصفحات تلقائياً
    // public int TotalPages =>
    //     (int)Math.Ceiling((double)TotalCount / PageSize);
}
