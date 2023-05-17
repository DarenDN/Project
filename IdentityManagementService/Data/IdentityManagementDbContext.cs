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
    public DbSet<RefreshToken> RefreshTokens { get; set; }
}
