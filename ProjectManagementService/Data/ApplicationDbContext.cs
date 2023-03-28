namespace ProjectManagementService.Data;

using Microsoft.EntityFrameworkCore;
using Models;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<TaskModel> TaskModels { get; set; }
    public DbSet<UserModel> UserModels { get; set; }
    public DbSet<UserRoleModel> UserRoleModels { get; set; }
    public DbSet<TaskTypeModel> TaskTypeModels { get; set; }
    public DbSet<TaskStatusModel> TaskStatusModels { get; set; }
    public DbSet<ProjectModel> ProjectModels { get; set; }
    public DbSet<DashboardModel> DashboardModels { get; set; }
}
