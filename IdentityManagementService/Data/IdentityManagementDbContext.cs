namespace IdentityManagementService.Data;

using Models;
using Microsoft.EntityFrameworkCore;

public class IdentityManagementDbContext : DbContext
{
    public IdentityManagementDbContext(DbContextOptions<IdentityManagementDbContext> appDbOptions) : base(appDbOptions)
    {
    }

    public DbSet<Identity> Identities { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<ProjectsRole> ProjectsRoles { get; set; }
}
