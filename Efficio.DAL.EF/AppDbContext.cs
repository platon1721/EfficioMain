
using Efficio.Domain.Departments;
using Efficio.Domain.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Efficio.DAL.EF;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
{
    public DbSet<AppRefreshToken> RefreshTokens => Set<AppRefreshToken>();
    public DbSet<Department> Departments { get; set; } = default!;
    public DbSet<DepartmentType> DepartmentTypes { get; set; } = default!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
}