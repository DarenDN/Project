namespace IdentityManagementService.Data;

using Models;
using Microsoft.EntityFrameworkCore;

public class IdentityManagementDbContext : DbContext
{
    public IdentityManagementDbContext(DbContextOptions<IdentityManagementDbContext> appDbOptions) : base(appDbOptions)
    {
    }

    public DbSet<Identity> Users { get; set; }
}
