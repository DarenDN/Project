using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ProjectManagementService.Data;
using ProjectManagementService.Services.Dashboard;
using ProjectManagementService.Services.Sprint;
using ProjectManagementService.Services.Project;
using ProjectManagementService.Services.Task;
using ProjectManagementService.Services.Role;
using ProjectManagementService.Services.ProductBacklog;
using ProjectManagementService.Services.BurndownChart;
using ProjectManagementService.Configurations;
using ProjectManagementService.Services.State;
using ProjectManagementService.Models;
using Microsoft.Extensions.Options;
using System.Data;
using ProjectManagementService.Services.Type;
using Microsoft.OpenApi.Any;

const string SecurityCfgTokenSection = "SecurityConfiguration:Token";

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RoleConfiguration>(builder.Configuration.GetSection(RoleConfiguration.ConfigurationName));
builder.Services.Configure<StateConfiguration>(builder.Configuration.GetSection(StateConfiguration.ConfigurationName));
builder.Services.Configure<TypeConfiguration>(builder.Configuration.GetSection(TypeConfiguration.ConfigurationName));

var connectionString = (Environment.GetEnvironmentVariable("MY_ENV_VAR") != "DOCKER")
    ? builder.Configuration.GetConnectionString("DataConnection")
    : builder.Configuration.GetConnectionString("DataConnectionDocker");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(connectionString);
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    opt => opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.Unicode.GetBytes(
                builder.Configuration.GetValue<string>(SecurityCfgTokenSection))),
        ValidateIssuer = false,
        ValidateAudience = false
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IDashboardService), typeof(DashboardService));
builder.Services.AddScoped(typeof(IProjectService), typeof(ProjectService));
builder.Services.AddScoped(typeof(ITaskService), typeof(TaskService));
builder.Services.AddScoped(typeof(IRoleService), typeof(RoleService));
builder.Services.AddScoped(typeof(IUserStoryService), typeof(UserStoryService));
builder.Services.AddScoped(typeof(IBurndownChartService), typeof(BurndownChartService));
builder.Services.AddScoped(typeof(ISprintService), typeof(SprintService));
builder.Services.AddScoped(typeof(IStateService), typeof(StateService));
builder.Services.AddScoped(typeof(ITypeService), typeof(TypeService));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.MapType<TimeSpan>(() => new OpenApiSchema
    {
        Type = "string",
        Example = new OpenApiString("000.00:00:00")
    });

    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Auth header",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var serviceProvider = app.Services.CreateAsyncScope())
{
    await FillBasicDataAsync(serviceProvider.ServiceProvider).ConfigureAwait(true);
}

await app.RunAsync().ConfigureAwait(true);

async System.Threading.Tasks.Task FillBasicDataAsync(IServiceProvider serviceProvider)
{
    var appDbContext = serviceProvider.GetService<ApplicationDbContext>();
    var roleCfg = serviceProvider.GetService<IOptions<RoleConfiguration>>().Value;
    var stateCfg = serviceProvider.GetService<IOptions<StateConfiguration>>().Value;
    var typeCfg = serviceProvider.GetService<IOptions<TypeConfiguration>>().Value;

    foreach(var role in roleCfg.BasicRoles)
    {
        var existingRole = await appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Id == role.Value);
        if (existingRole is null)
        {
            await appDbContext.UserRoles.AddAsync(new UserRole { Id = role.Value, Name = role.Key });
            foreach (var project in appDbContext.Projects)
            {
                await appDbContext.ProjectsRoles.AddAsync(new ProjectsRole { ProjectId = project.Id, RoleId = role.Value });
            }
        }
        else if (!string.Equals(existingRole.Name, role.Key))
        {
            existingRole.Name = role.Key;
            appDbContext.UserRoles.Update(existingRole);
        }
    }

    foreach(var state in stateCfg.BasicStates)
    {
        var existingState = await appDbContext.TaskStates.FirstOrDefaultAsync(r => r.Id == state.Value);
        if (existingState is null)
        {
            await appDbContext.TaskStates.AddAsync(new TaskState { Id = state.Value, Name = state.Key });
        }
        else if (!string.Equals(existingState.Name, state.Key))
        {
            existingState.Name = state.Key;
            appDbContext.TaskStates.Update(existingState);
        }
    }

    var newStateRelationships = new List<StateRelationship>();
    var stateFields = stateCfg.BasicStates;

    foreach (var relationship in stateCfg.BasicRelationships)
    {
        stateFields.TryGetValue(relationship.Key, out var mainGuid);
        foreach (var nextState in relationship.Value)
        {
            stateFields.TryGetValue(nextState, out var nextGuid);
            if (await appDbContext.StateRelationships.AnyAsync(sr => sr.StateCurrent == mainGuid && sr.StateNext == nextGuid))
            {
                continue;
            }

            newStateRelationships.Add(new StateRelationship
            {
                StateCurrent = mainGuid,
                StateNext = nextGuid
            });
        }
    }

    if (newStateRelationships.Any())
    {
        await appDbContext.StateRelationships.AddRangeAsync(newStateRelationships);
    }

    foreach(var type in typeCfg.BasicTypes)
    {
        var existingType = await appDbContext.TaskTypes.FirstOrDefaultAsync(r => r.Id == type.Value);
        if (existingType is null)
        {
            await appDbContext.TaskTypes.AddAsync(new TaskType { Id = type.Value, Name = type.Key });
        }
        else if (!string.Equals(existingType.Name, type.Key))
        {
            existingType.Name = type.Key;
            appDbContext.TaskTypes.Update(existingType);
        }
    }

    await appDbContext.SaveChangesAsync();
}