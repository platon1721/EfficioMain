using Base.Contracts;
using Efficio.Domain.Departments;
using Efficio.Domain.Identity;
using Efficio.Domain.Security;
using Efficio.Domain.Tenants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Efficio.DAL.EF;

public class EfficioDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    private readonly IUserContext? _userContext;
    private readonly IClock _clock;

    // Security (global)
    public DbSet<Module> Modules { get; set; } = default!;
    public DbSet<Permission> Permissions { get; set; } = default!;

    // Tenants
    public DbSet<Tenant> Tenants { get; set; } = default!;
    public DbSet<TenantModule> TenantModules { get; set; } = default!;
    public DbSet<UserTenantMembership> UserTenantMemberships { get; set; } = default!;

    // Departments
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<DepartmentType> DepartmentTypes { get; set; } = default!;
    public DbSet<DepartmentInDepartment> DepartmentInDepartments { get; set; } = default!;

    // Security (department-scoped)
    public DbSet<Role> Roles { get; set; } = default!;
    public DbSet<RolePermission> RolePermissions { get; set; } = default!;
    public DbSet<UserDepartmentRole> UserDepartmentRoles { get; set; } = default!;

    // Identity
    public DbSet<AppRefreshToken> RefreshTokens { get; set; } = default!;

    public EfficioDbContext(DbContextOptions<EfficioDbContext> options, IUserContext? userContext, IClock clock)
        : base(options)
    {
        _userContext = userContext;
        _clock = clock;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Disable cascade delete globally
        foreach (var relationship in builder.Model
                     .GetEntityTypes()
                     .SelectMany(e => e.GetForeignKeys()))
        {
            relationship.DeleteBehavior = DeleteBehavior.Restrict;
        }

        // ===================== Module =====================
        builder.Entity<Module>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // ===================== Permission =====================
        builder.Entity<Permission>(entity =>
        {
            entity.HasIndex(e => e.Key).IsUnique();

            entity.HasOne(e => e.Module)
                .WithMany()
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== Tenant =====================
        builder.Entity<Tenant>(entity =>
        {
            entity.HasIndex(e => e.Code).IsUnique();
        });

        // ===================== TenantModule =====================
        builder.Entity<TenantModule>(entity =>
        {
            entity.HasIndex(e => new { e.TenantRootDepartmentId, e.ModuleId }).IsUnique();

            entity.HasOne(e => e.Module)
                .WithMany()
                .HasForeignKey(e => e.ModuleId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== UserTenantMembership =====================
        builder.Entity<UserTenantMembership>(entity =>
        {
            entity.HasIndex(e => new { e.TenantRootDepartmentId, e.UserId }).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== DepartmentType =====================
        builder.Entity<DepartmentType>(entity =>
        {
            entity.HasIndex(e => new { e.TenantRootDepartmentId, e.Name }).IsUnique();
        });

        // ===================== Department =====================
        builder.Entity<Department>(entity =>
        {
            entity.HasOne(e => e.DepartmentType)
                .WithMany()
                .HasForeignKey(e => e.DepartmentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== DepartmentInDepartment =====================
        builder.Entity<DepartmentInDepartment>(entity =>
        {
            entity.HasIndex(e => new { e.ParentDepartmentId, e.ChildDepartmentId }).IsUnique();

            entity.HasOne(e => e.ParentDepartment)
                .WithMany(d => d.ChildDepartmentLinks)
                .HasForeignKey(e => e.ParentDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.ChildDepartment)
                .WithMany(d => d.ParentDepartmentLinks)
                .HasForeignKey(e => e.ChildDepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== Role =====================
        builder.Entity<Role>(entity =>
        {
            entity.HasIndex(e => new { e.DepartmentId, e.Name }).IsUnique();

            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== RolePermission =====================
        builder.Entity<RolePermission>(entity =>
        {
            entity.HasIndex(e => new { e.RoleId, e.PermissionId }).IsUnique();

            entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Permission)
                .WithMany()
                .HasForeignKey(e => e.PermissionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== UserDepartmentRole =====================
        builder.Entity<UserDepartmentRole>(entity =>
        {
            // One user can have only one role per department
            entity.HasIndex(e => new { e.DepartmentId, e.UserId }).IsUnique();

            entity.HasOne(e => e.Role)
                .WithMany()
                .HasForeignKey(e => e.RoleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Department)
                .WithMany()
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // ===================== Soft Delete Global Filter =====================
        builder.Entity<Tenant>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<TenantModule>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<UserTenantMembership>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<DepartmentType>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<UserDepartmentRole>().HasQueryFilter(e => !e.IsDeleted);
        builder.Entity<Permission>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessAuditableEntities();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ProcessAuditableEntities();
        return base.SaveChanges();
    }

    private void ProcessAuditableEntities()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is IDomainMeta && 
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var userId = _userContext?.UserId ?? Guid.Empty;

            if (entry.State == EntityState.Added)
            {
                var createMethod = entry.Entity.GetType().GetMethod("Create", 
                    new[] { typeof(Guid), typeof(IClock) });
                createMethod?.Invoke(entry.Entity, new object[] { userId, _clock });
            }
            else if (entry.State == EntityState.Modified)
            {
                var updateMethod = entry.Entity.GetType().GetMethod("Update", 
                    new[] { typeof(Guid), typeof(IClock) });
                updateMethod?.Invoke(entry.Entity, new object[] { userId, _clock });

                // Prevent overwriting Created fields
                entry.Property("CreatedById").IsModified = false;
                entry.Property("CreatedAt").IsModified = false;
                entry.Property("CreatedByName").IsModified = false;
            }
        }

        // Process soft delete
        var deletedEntries = ChangeTracker.Entries()
            .Where(e => e.Entity is IDomainSoftDelete && e.State == EntityState.Deleted);

        foreach (var entry in deletedEntries)
        {
            entry.State = EntityState.Modified;
            var deleteMethod = entry.Entity.GetType().GetMethod("Delete",
                new[] { typeof(Guid), typeof(IClock) });
            deleteMethod?.Invoke(entry.Entity, new object[] { _userContext?.UserId ?? Guid.Empty, _clock });
        }
    }
}