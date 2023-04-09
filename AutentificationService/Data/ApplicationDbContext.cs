namespace AutentificationService.Data;

using Models;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> appDbOptions) : base(appDbOptions)
    {
    }

    public DbSet<User> Users { get; set; }
}
