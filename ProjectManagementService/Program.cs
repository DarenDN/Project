using ProjectManagementService.Data;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection"));
});

// TODO What exactly it does?
builder.Services.AddControllers();

// TODO do we need interfaces?
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ProjectService>();
builder.Services.AddScoped<TaskService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
