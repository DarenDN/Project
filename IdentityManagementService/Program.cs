using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;
using IdentityManagementService.Configurations;
using IdentityManagementService.Data;
using IdentityManagementService.Services.IdentityManagement;

const string AuthConnectionCfgSection = "AuthConnection";
const string SecurityCfgTokenSection = "SecurityConfiguration:Token";


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<IdentityManagementDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(AuthConnectionCfgSection));
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
    typeof(IdentityManagementService.Services.IdentityManagement.AuthService));
builder.Services.AddScoped(typeof(IIdentityManagementService),
    typeof(IdentityManagementService.Services.IdentityManagement.IdentityManagementService));

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();
