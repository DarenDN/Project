using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using Swashbuckle.AspNetCore.Filters;
using IdentityManagementService.Configurations;
using IdentityManagementService.Data;
using IdentityManagementService.Services.Auth;

const string AuthConnectionCfgSection = "AuthConnection";
const string TestUserConfiguration = "testUsersCfg.json";

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile(TestUserConfiguration);

builder.Services.AddDbContext<AppDbContext>(options =>
{
#if DEBUG
    options.UseNpgsql(builder.Configuration.GetConnectionString(AuthConnectionCfgSection));
#else
    options.UseNpgsql(builder.Configuration.GetConnectionString($"{AuthConnectionCfgSection}Docker"));
#endif
});

builder.Services.Configure<SecurityConfiguration>(builder.Configuration.GetSection(SecurityConfiguration.ConfigurationName));
builder.Services.Configure<TestUsersConfiguration>(builder.Configuration.GetSection(TestUsersConfiguration.ConfigurationName));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
    opt => opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.Unicode.GetBytes(
                builder.Configuration.GetValue<string>($"{SecurityConfiguration.ConfigurationName}:Token"))),
        ValidateIssuer = false,
        ValidateAudience = false
    });

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped(typeof(IAuthService),
    typeof(AuthService));
builder.Services.AddScoped(typeof(IdentityManagementService.Services.IdentityManagement.IIdentityManagementService),
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

if(bool.TryParse(Environment.GetEnvironmentVariable("MIGRATE"), out var migrate) && migrate)
{
    using (var scope = app.Services.CreateScope())
    {
        await scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.MigrateAsync();
    }
}

app.Run();
