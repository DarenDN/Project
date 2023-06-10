namespace IdentityManagementService.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public class AppDbContext : DbContext
{
    public AppDbContext(
        DbContextOptions<AppDbContext> appDbOptions) : base(appDbOptions)
    {
    }

    public DbSet<Identity> Identities { get; set; }
    public DbSet<UserInfo> UserInfos { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
