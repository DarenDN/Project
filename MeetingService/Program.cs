using MeetingService.Hubs;
using MeetingService.Services.CacheService;
using MeetingService.Services.MeetingService;
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

builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton(typeof(ICacheService),typeof(RedisCacheService));
builder.Services.AddScoped(typeof(IMeetingService),typeof(MeetingService.Services.MeetingService.MeetingService));

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
 * Одна "доска" для оценки
 * 1. На доске есть возможность выбора того, что будет оцениваться, т.е. либо эпик (сторис), либо задача
 * 1.1. 
 * 
 * 2. На доске Несколько пользователей
 * У каждого пользователя варианты оценки
 * Для всех текущая оценка закрыта до момента, когда все проголосуют и админ митинга прожмет "вскрытие" результатов
 * 
 * Доска как объект состоит из: списка пользователей с их текущей оценкой, текущего объекта оценивания
 * Пользователь + оценка = Дтошка Юзер + оценка
 * 
 * 3. Что видит юзер
 * Текущий объект оценивания
 * Сколько участников
 * Участников и их имена/ники
 * Скрытую/открытую оценку
 * 
 * -. Возможность выбора оценки: время или поинты сложности - второстепенное, дефолт будет поинты атм
 * -. Клик на текущий объект оценивания откроет вкладку с инфой, т.е. если это задача - откроется задача, если эпик - откроется эпик
 * 
 * 
 * 
 */