using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
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
using ProjectManagementService.Services.Type;
using ProjectManagementService.Data;

const string SecurityCfgTokenSection = "SecurityConfiguration:Token";
const string DefaultDataCfg = "defaultData.json";
const string TestDataCfg = "testData.json";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(DefaultDataCfg);
builder.Configuration.AddJsonFile(TestDataCfg);
builder.Services.AddCors(options => options.AddPolicy("CorsPolicy", builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));
builder.Services.Configure<RoleConfiguration>(builder.Configuration.GetSection(RoleConfiguration.ConfigurationName));
builder.Services.Configure<StateConfiguration>(builder.Configuration.GetSection(StateConfiguration.ConfigurationName));
builder.Services.Configure<TypeConfiguration>(builder.Configuration.GetSection(TypeConfiguration.ConfigurationName));
//builder.Services.Configure<TestDataConfiguration>(builder.Configuration.GetSection(TestDataConfiguration.ConfigurationName));

builder.Services.AddDbContext<AppDbContext>(options =>
{
#if DEBUG
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection"));
#else
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnectionDocker"));
#endif
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
app.UseCors("CorsPolicy");
//app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1"));
}

if (bool.TryParse(Environment.GetEnvironmentVariable("MIGRATE"), out var migrate) && migrate)
{
    using (var scope = app.Services.CreateScope())
    {
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    }
}

app.Run();
