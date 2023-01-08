using ProjectManagementService.Data;
using Microsoft.EntityFrameworkCore;
using Autofac;

var builder = WebApplication.CreateBuilder(args);

// TODO if db connection is correct
var dbContext = builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseNpgsql(builder.Configuration.GetConnectionString("DataConnection"))).First();

InitContainer();

// TODO where to set it?!

var app = builder.Build();

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

void InitContainer()
{
    var containerBuilder = new ContainerBuilder();
    containerBuilder.RegisterInstance<ApplicationDbContext>((ApplicationDbContext)dbContext.ImplementationInstance).SingleInstance();
    Application.Container = containerBuilder.Build();
}
