using MeetingService.Hubs;
using MeetingService.Services.CacheService;
using MeetingService.Services.MeetingService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

const string SecurityCfgTokenSection = "SecurityConfiguration:Token";
const string RedisCfgInstanceNameSection = "RedisConfiguration:InstanceName";
const string RedisConnectionStringSection = "Redis";
const string SignalRCfgRouteSection = "SignalRConfiguration:Route";

var builder = WebApplication.CreateBuilder(args);

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

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString(RedisConnectionStringSection);
    options.InstanceName = builder.Configuration.GetValue<string>(RedisCfgInstanceNameSection);
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(ICacheService),typeof(RedisCacheService));
builder.Services.AddScoped(typeof(IMeetingService),typeof(MeetingService.Services.MeetingService.MeetingService));

builder.Services.AddSignalR(cfg =>
{
    cfg.EnableDetailedErrors = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseEndpoints(e =>
{
    e.MapHub<MeetingHub>(app.Configuration.GetValue<string>(SignalRCfgRouteSection));
});

app.MapControllers();

await app.RunAsync();
