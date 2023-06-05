namespace ProjectManagementService.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using ProjectManagementService.Configurations;

public sealed class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Models.Task> Tasks { get; set; }
    public DbSet<TaskType> TaskTypes { get; set; }
    public DbSet<TaskState> TaskStates { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<Dashboard> Dashboards { get; set; }
    public DbSet<ProjectsIdentity> ProjectsIdentities { get; set; }
    public DbSet<ProjectsRole> ProjectsRoles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserStory> UserStories { get; set; }
    public DbSet<Sprint> Sprints { get; set; }
    public DbSet<StateRelationship> StateRelationships { get; set; }
}
