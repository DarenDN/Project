namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Models;
using ProjectManagementService.Data;

[Route("api/[controller]")]
[ApiController]
public sealed class TestController : ControllerBase
{
    private readonly Data.AppDbContext _applicationDbContext;

    public TestController(
        Data.AppDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    [HttpPost]
    [Route(nameof(FillTestDataAsync))]
    public async Task<ActionResult> FillTestDataAsync()
    {
        try
        {
            await TryFillTestDataAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    private async System.Threading.Tasks.Task TryFillTestDataAsync()
    {
        var testProject = new Project
        {
            Id = Guid.Parse("fa98f72f-4cce-4350-a306-6d1c61bc7b06"),
            Description = "Проект CheckIt - амбициозный проект! Проект, в рамках которого МЫ создаем приложение для мониторинга задач и их командной оценки...",
            Title = "CheckIt"
        };

        var testSprint = new Sprint
        {
            Id = Guid.Parse("2e39035b-49ec-445c-aca3-e933d93015d5"),
            Name = "Начальный спринт",
            DateStart = DateTime.Now,
            DateEnd = DateTime.Now.AddDays(14),
            Description = "В рамках текущего спринта в первую очередь нужно реализовать авторизацию",
            ProjectId = testProject.Id
        };

        var testProjectsIdentities =
            new List<ProjectsIdentity>
            {
                new ProjectsIdentity
                {
                    IdentityId = Guid.Parse("41e3060b-c024-4b94-9eec-de26f72b839a"),
                    ProjectId = testProject.Id
                },
                new ProjectsIdentity
                {
                    IdentityId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id
                },
                new ProjectsIdentity
                {
                    IdentityId = Guid.Parse("4195c605-c167-4ac3-ad1b-bc3cc4afd008"),
                    ProjectId = testProject.Id
                },
                new ProjectsIdentity
                {
                    IdentityId = Guid.Parse("93c87ea0-797e-4f7a-a99e-cc71a0267164"),
                    ProjectId = testProject.Id
                },
                new ProjectsIdentity
                {
                    IdentityId = Guid.Parse("51783eb8-c5fb-4eba-bee0-2bdaa9aefe93"),
                    ProjectId = testProject.Id
                },
            };

        var taskStates = await _applicationDbContext.TaskStates.ToDictionaryAsync(k => k.Name);
        var taskTypes = await _applicationDbContext.TaskTypes.ToDictionaryAsync(k => k.Name);

        var testTasks = new List<Models.Task>
            {
                new Models.Task
                {
                    Title = "Двухфакторная аутентификация",
                    Description = "Создание механизма проверки логина и пароля пользователя в сочетании с кодом, отправляемым по СМС или генерируемым специальным приложением на устройстве",
                    State = taskStates.GetValueOrDefault("Оценка"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    CreatorId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Автоматическое создание уникальных паролей",
                    Description = "Разработка механизма автоматической генерации и отправки уникальных паролей пользователям по электронной почте или СМС-сообщениям для повышения безопасности.",
                    State = taskStates.GetValueOrDefault("Оценка"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    CreatorId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Вход через социальные сети",
                    Description = "Реализация возможности авторизации пользователей через профили социальных сетей (например, Facebook, Twitter, LinkedIn) для упрощения процесса входа и повышения удобства использования сайта.",
                    State = taskStates.GetValueOrDefault("Анализ"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    CreatorId = Guid.Parse("4195c605-c167-4ac3-ad1b-bc3cc4afd008"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Контроль доступа в зависимости от роли пользователя",
                    Description = "Организация системы контроля доступа к ресурсам сайта на основе ролей пользователей (например, администратор, модератор, пользователь) для обеспечения безопасности и управления правами доступа.",
                    State = taskStates.GetValueOrDefault("К работе"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    EstimationInPoints = 8,
                    CreatorId = Guid.Parse("4195c605-c167-4ac3-ad1b-bc3cc4afd008"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Подтверждение email адреса",
                    Description = "Создание механизма подтверждения email адреса при регистрации новых пользователей для повышения безопасности и предотвращения создания фиктивных аккаунтов.",
                    State = taskStates.GetValueOrDefault("К работе"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    EstimationInPoints = 5,
                    CreatorId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id, SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Автоматический выход из системы",
                    Description = "Реализация механизма автоматического выхода пользователя из системы при закрытии браузера или завершении работы на устройстве для обеспечения безопасности и предотвращения несанкционированного доступа.",
                    State = taskStates.GetValueOrDefault("Работа"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    EstimationInPoints = 3,
                    EstimationInTime = TimeSpan.FromDays(2),
                    PerformerId = Guid.Parse("93c87ea0-797e-4f7a-a99e-cc71a0267164"),
                    CreatorId = Guid.Parse("4195c605-c167-4ac3-ad1b-bc3cc4afd008"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Блокировка аккаунта",
                    Description = "Организация системы блокировки аккаунта пользователя при попытке входа с неверными учетными данными или при обнаружении подозрительной активности в аккаунте для защиты данных пользователя.",
                    State = taskStates.GetValueOrDefault("К тестированию"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    EstimationInPoints = 3,
                    EstimationInTime = TimeSpan.FromHours(7),
                    CreatorId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id, SprintId = testSprint.Id
                },
                new Models.Task
                {
                    Title = "Восстановление доступа к учетной записи",
                    Description = "Создание механизма восстановления доступа к учетной записи пользователя в случае утери или забытия пароля для повышения удобства использования сайта.",
                    State = taskStates.GetValueOrDefault("К тестированию"),
                    Type = taskTypes.GetValueOrDefault("Пользовательская история"),
                    EstimationInPoints = 2,
                    EstimationInTime = TimeSpan.FromHours(7),
                    CreatorId = Guid.Parse("6c3e73ab-5636-486e-8754-c591e915fa5f"),
                    ProjectId = testProject.Id, SprintId = testSprint.Id,
                },
                new Models.Task
                {
                    Title = "Развернуть тестовую и прод схемы для тестирования",
                    Description = "Необходимо развернуть тесовую схему на сервере, так же развернуть Docker и приготовить соответствующие скрипты для автоматизации развертываний приложения на схеме.",
                    State = taskStates.GetValueOrDefault("Завершено"),
                    Type = taskTypes.GetValueOrDefault("Задача"),
                    EstimationInPoints = 8,
                    CreatorId = Guid.Parse("41e3060b-c024-4b94-9eec-de26f72b839a"),
                    ProjectId = testProject.Id,
                    SprintId = testSprint.Id
                },
            };

        await _applicationDbContext.Projects.AddAsync(testProject);
        await _applicationDbContext.Sprints.AddAsync(testSprint);
        await _applicationDbContext.ProjectsIdentities.AddRangeAsync(testProjectsIdentities);
        await _applicationDbContext.Tasks.AddRangeAsync(testTasks);

        await _applicationDbContext.SaveChangesAsync();
    }
}
