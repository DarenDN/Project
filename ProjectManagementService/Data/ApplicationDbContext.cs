namespace ProjectManagementService.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Task> Tasks { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<TaskStatus> TaskStatuss { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
}
