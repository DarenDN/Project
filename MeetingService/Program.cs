using MeetingService.Hubs;
using MeetingService.Services.Implementations;
using MeetingService.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
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
builder.Services.AddSingleton(typeof(ICacheService),typeof(RedisCacheService));
builder.Services.AddSingleton(typeof(IMeetingService),typeof(MeetingService.Services.Implementations.MeetingService));

builder.Services.AddSignalR(cfg =>
{
    cfg.EnableDetailedErrors = true;
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(e =>
{
    e.MapHub<MeetingHub>(app.Configuration.GetValue<string>(SignalRCfgRouteSection));
});

app.MapControllers();

await app.RunAsync();


// TODO
/*
 * ���� "�����" ��� ������
 * 1. �� ����� ���� ����������� ������ ����, ��� ����� �����������, �.�. ���� ���� (������), ���� ������
 * 1.1. 
 * 
 * 2. �� ����� ��������� �������������
 * � ������� ������������ �������� ������
 * ��� ���� ������� ������ ������� �� �������, ����� ��� ����������� � ����� ������� ������� "��������" �����������
 * 
 * ����� ��� ������ ������� ��: ������ ������������� � �� ������� �������, �������� ������� ����������
 * ������������ + ������ = ������ ���� + ������
 * 
 * 3. ��� ����� ����
 * ������� ������ ����������
 * ������� ����������
 * ���������� � �� �����/����
 * �������/�������� ������
 * 
 * -. ����������� ������ ������: ����� ��� ������ ��������� - ��������������, ������ ����� ������ ���
 * -. ���� �� ������� ������ ���������� ������� ������� � �����, �.�. ���� ��� ������ - ��������� ������, ���� ���� - ��������� ����
 * 
 * 
 * 
 */