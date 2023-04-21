using IdentityManagementService.Configurations;
using IdentityManagementService.Data;
using IdentityManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

const string AuthConnectionCfgSection = "AuthConnection";
const string SecurityCfgTokenSection = "SecurityConfiguration:Token";


var builder = WebApplication.CreateBuilder(args);

// TODO if db connection is correct
builder.Services.AddDbContext<IdentityManagementDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString(AuthConnectionCfgSection));
});

builder.Services.Configure<SecurityConfiguration>(builder.Configuration.GetSection("Security"));

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

// TODO add services
builder.Services.AddSingleton(
    typeof(IIdentityManagementService), 
    typeof(IdentityManagementService.Services.Implementations.IdentityManagementService));

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
