namespace ProjectManagementService.Data;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Models;
using Configurations;

public sealed class AppDbContext : DbContext
{
    private RoleConfiguration _roleConfiguration;
    private StateConfiguration _stateConfiguration;
    private TypeConfiguration _typeConfiguration;

    public AppDbContext(
        DbContextOptions<AppDbContext> options,
        IOptions<TypeConfiguration> options1,
        IOptions<StateConfiguration> options2,
        IOptions<RoleConfiguration> options3) : base(options)
    {
        _roleConfiguration = options3.Value;
        _stateConfiguration = options2.Value;
        _typeConfiguration = options1.Value;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        FillDefaultData(modelBuilder);
    }

    private void FillDefaultData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>()
            .HasData(_roleConfiguration.BasicRoles
                        .Select(r => new UserRole { Id = r.Value, Name = r.Key }));

        var taskStates = _stateConfiguration.BasicStates
                        .Select(s => new TaskState { Id = s.Value, Name = s.Key })
                        .ToDictionary(k => k.Name);

        modelBuilder.Entity<TaskState>()
            .HasData(taskStates.Values);

        modelBuilder.Entity<StateRelationship>()
            .HasData(_stateConfiguration.BasicRelationships
                        .SelectMany(sr => sr.Value.Select(depend => new StateRelationship
                        {
                            StateCurrent = taskStates.GetValueOrDefault(sr.Key).Id,
                            StateNext = taskStates.GetValueOrDefault(depend).Id,
                        })));

        var taskTypes = _typeConfiguration.BasicTypes
                            .Select(t => new TaskType
                            {
                                Id = t.Value,
                                Name = t.Key,
                            })
                            .ToDictionary(k => k.Name);
        modelBuilder.Entity<TaskType>()
            .HasData(taskTypes.Values);
    }
}
