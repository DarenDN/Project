namespace DataBaseContract.Data;

using Models.ScrumRelated;
using Models.UserRelated;
using Microsoft.EntityFrameworkCore;

public class DataBaseContext : DbContext
{
    public DataBaseContext(DbContextOptions<DataBaseContext> appDbOptions) : base(appDbOptions)
    {
    }

    public DbSet<Identity> Identities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    public DbSet<Project> Projects { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskStatus> TaskStatuses { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
}
