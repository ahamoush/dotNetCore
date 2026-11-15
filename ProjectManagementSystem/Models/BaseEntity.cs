// خصائص مشتركة بين الجداول (Id + تواريخ + Soft Delete)
public abstract class BaseEntity
{
    public int Id { get; set; }

    // تاريخ الإنشاء (UTC)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // تاريخ آخر تعديل (null لو لسه ما اتعدلش)
    public DateTime? UpdatedAt { get; set; }

    // Soft Delete: true = محذوف منطقياً، لكنه موجود في الداتابيز
    public bool IsDeleted { get; set; } = false;

    // متى اتعمل Soft Delete
    public DateTime? DeletedAt { get; set; }
}
