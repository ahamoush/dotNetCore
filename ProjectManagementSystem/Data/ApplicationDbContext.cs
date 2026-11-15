using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

// IdentityDbContext يضيف جداول Identity تلقائياً:
// AspNetUsers, AspNetRoles, AspNetUserRoles, AspNetUserClaims, AspNetRoleClaims, ...
public class ApplicationDbContext
    : IdentityDbContext<ApplicationUser, ApplicationRole, string>
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

    // قبل الحفظ: نعبّي CreatedAt / UpdatedAt للكيانات اللي ترث BaseEntity
    public override Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        var utcNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = utcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Entity.UpdatedAt = utcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // مهم جداً: يستدعي إعدادات جداول Identity
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.IsActive)
                .HasDefaultValue(true);

            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        });

        modelBuilder.Entity<Project>(entity =>
        {
            // Soft Delete filter
            entity.HasQueryFilter(p => !p.IsDeleted);

            entity.Property(p => p.Name)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(p => p.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(p => p.Status)
                .HasDefaultValue(ProjectStatus.New)
                .HasSentinel((ProjectStatus)0);

            entity.Property(p => p.IsDeleted)
                .HasDefaultValue(false);

            entity.HasMany(p => p.Tasks)
                .WithOne(t => t.Project)
                .HasForeignKey(t => t.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ProjectTask>()
            .HasQueryFilter(t => !t.Project!.IsDeleted);
    }
}
