using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

// ApplicationDbContext = الجسر بين الكود وقاعدة البيانات
// كل DbSet يمثل جدول في SQL Server
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // جدول المشاريع
    public DbSet<Project> Projects => Set<Project>();

    // جدول المهام
    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();

    // هنا نضبط قواعد الجداول والعلاقات والفلاتر
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Project>(entity =>
        {
            // Global Query Filter:
            // أي استعلام على Projects يستبعد الصفوف المحذوفة Soft Delete تلقائياً
            // يعني: WHERE IsDeleted = 0 بدون ما نكتب الشرط في كل Method
            entity.HasQueryFilter(p => !p.IsDeleted);

            entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            // القيمة الافتراضية لحالة المشروع = New
            // HasSentinel: يخبر EF إن القيمة 0 معناها "لسه ما اتحددتش"
            // عشان يستخدم Default Value من الداتابيز بدل ما يكتب 0
            entity.Property(p => p.Status)
                .HasDefaultValue(ProjectStatus.New)
                .HasSentinel((ProjectStatus)0);

            entity.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            // علاقة One-to-Many:
            // Project واحد له Tasks كثيرة
            // Cascade: لو اتمسح المشروع، مهامه تتمسح معه
            entity.HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // لو المشروع Soft Deleted، مهامه كمان ما تظهرش في الاستعلامات العادية
        modelBuilder.Entity<ProjectTask>()
            .HasQueryFilter(t => !t.Project!.IsDeleted);
    }
}
