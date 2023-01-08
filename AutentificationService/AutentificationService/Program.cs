using ProjectManagementService.Data;
using Microsoft.EntityFrameworkCore;
using Autofac;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// TODO if db connection is correct
var dbContext = builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection"))).First();

var containerBuilder = new ContainerBuilder();
containerBuilder.RegisterInstance<ApplicationDbContext>((ApplicationDbContext)dbContext.ImplementationInstance).SingleInstance();

// TODO where to set it?!
container = containerBuilder.Build();

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
