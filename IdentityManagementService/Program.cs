using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using IdentityManagementService.Configurations;
using IdentityManagementService.Data;
using IdentityManagementService.Services.IdentityManagement;
using IdentityManagementService.Services.Auth;
using Microsoft.OpenApi.Any;
using Microsoft.Extensions.Hosting;
using System;

const string AuthConnectionCfgSection = "AuthConnection";
const string SecurityCfgTokenSection = "SecurityConfiguration:Token";


var builder = WebApplication.CreateBuilder(args);

var connectionString = (Environment.GetEnvironmentVariable("MY_ENV_VAR") != "DOCKER")
    ? builder.Configuration.GetConnectionString(AuthConnectionCfgSection)
    : builder.Configuration.GetConnectionString($"{AuthConnectionCfgSection}Docker");

builder.Services.AddDbContext<IdentityManagementDbContext>(options =>
{
#if DEBUG
    options.UseNpgsql(builder.Configuration.GetConnectionString(AuthConnectionCfgSection));
#else
    options.UseNpgsql(builder.Configuration.GetConnectionString($"{AuthConnectionCfgSection}Docker"));
#endif
});

builder.Services.Configure<SecurityConfiguration>(builder.Configuration.GetSection("SecurityConfiguration"));

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
builder.Services.AddScoped(typeof(IAuthService),
    typeof(AuthService));
builder.Services.AddScoped(typeof(IIdentityManagementService),
    typeof(IdentityManagementService.Services.IdentityManagement.IdentityManagementService));

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
        Type = SecuritySchemeType.ApiKey
    });

    opt.OperationFilter<SecurityRequirementsOperationFilter>();
});

var app = builder.Build();

//app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "app v1"));
}

using (var scope = app.Services.CreateScope())
{
    await scope.ServiceProvider.GetRequiredService<IdentityManagementDbContext>().Database.MigrateAsync();
}

app.Run();
