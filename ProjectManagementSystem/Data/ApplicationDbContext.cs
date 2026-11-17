using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjectManagementSystem.Models;

namespace ProjectManagementSystem.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    //IdentityDbContext :
    // هو الكلاس الذي يتم استخدامه للتعامل مع قاعدة البيانات لنظام الهويات.
    // ApplicationUser :
    // هو الكلاس الذي يتم استخدامه للتعامل مع قاعدة البيانات لنظام الهويات.
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbContextOptions :
    // هو الخيارات التي يتم تمريرها إلى السياق لتحديد كيفية التعامل مع قاعدة البيانات.  
    // base(options) :
    // هو الطريقة التي يستخدمها السياق لتمرير الخيارات إلى الأساس.
    // ApplicationDbContext :
    // هو الكلاس الذي يتم استخدامه للتعامل مع قاعدة البيانات لنظام الهويات.

    public DbSet<Project> Projects => Set<Project>();

    public DbSet<ProjectTask> Tasks => Set<ProjectTask>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FullName)
                .HasMaxLength(100)
                .IsRequired();
        });

        modelBuilder.Entity<Project>(entity =>
        {
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
