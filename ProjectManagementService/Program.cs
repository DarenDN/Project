using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using ProjectManagementService.Data;
using ProjectManagementService.Services.Dashboard;
using ProjectManagementService.Services.Project;
using ProjectManagementService.Services.Task;
using ProjectManagementService.Services.Role;
using ProjectManagementService.Services.ProductBacklog;
using ProjectManagementService.Services.BurndownChart;

const string SecurityCfgTokenSection = "SecurityConfiguration:Token";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection"));
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Auth header",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
