namespace ProjectManagementService.Services.Task;

using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Task;
using Task = System.Threading.Tasks.Task;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Configurations;
using Models;

public sealed class TaskService : ITaskService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly StateConfiguration _stateCfg;

    public TaskService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IOptions<StateConfiguration> options)
    {
        _appDbContext = appDbContext;
        this._httpContextAccessor = httpContextAccessor;
        _stateCfg = options.Value;
    }

    public async Task CreateTaskAsync(CreateTaskDto newTaskDto)
    {
        var type = await _appDbContext.TaskTypes.FirstOrDefaultAsync(t => t.Id == newTaskDto.TypeId);
        var state = await GetDefaultTaskStateAsync();
        var newTask = new Models.Task
        {
            Title = newTaskDto.Title,
            Description = newTaskDto.Description,
            SprintId = newTaskDto.SprintId,
            State = state,
            Type = type
        };

        await _appDbContext.Tasks.AddAsync(newTask);
        await TrySaveChangesAsync();
    }

    public async Task UpdateTaskAsync(TaskUpdateDto taskDto)
    {
        var task = await GetTaskByIdAsync(taskDto.Id);

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.SprintId = taskDto.SprintId;
        // TODO chagne status and type
        // task.State 
        // task.Type
        task.PerformerId = taskDto.PerformerId;

        await UpdateAndSaveTaskAsync(task);
    }

    public async Task DeleteTaskAsync(Guid taskId)
    {
        var neededTask = await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId);
        if (neededTask is null)
        {
            // TODO exception
            throw new Exception();
        }

        _appDbContext.Tasks.Remove(neededTask);
        await TrySaveChangesAsync();
    }

    public async Task<IEnumerable<TaskShortInfoDto>> GetTasksAsync()
    {
        var tasks = await _appDbContext.Tasks
            .Include(t => t.State)
            .Include(t => t.Type)
            .Select(t => new TaskShortInfoDto(t.Id, t.Title, t.State.Name, t.Type.Name, t.PerformerId, t.CreatorId))
            .ToListAsync();

        return tasks;
    }

    private async Task<Guid?> GetCurrentSprintIdAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var today = DateTime.Now;
        var currentSprint = await _appDbContext.Sprints
            .FirstOrDefaultAsync(s => s.ProjectId == projectId 
                                             && s.DateStart <= today
                                             && s.DateEnd >= today);
        return currentSprint?.Id;
    }

    public async Task<TaskDataDto> GetTaskAsync(Guid taskId)
    {
        var task = await _appDbContext.Tasks
            .Where(t => t.Id == taskId)
            .Include(t=>t.State)
            .Include(t=>t.Type)
            .FirstOrDefaultAsync();

        if (task is null)
        {
            // TODO exception
            throw new Exception();
        }

        return new TaskDataDto(
            task.Id, 
            task.Title, 
            task.Description,
            task.State.Name,
            task.Type.Name,
            task.PerformerId,
            task.CreatorId);
    }

    public async Task ChangeStateAsync(Guid taskId, Guid statusId) 
    { 
        var task = await GetTaskByIdAsync(taskId);
        var status = await _appDbContext.TaskStates.FirstOrDefaultAsync(s => s.Id == statusId);
        task.State = status;
        await UpdateAndSaveTaskAsync(task);
    }

    public async Task ChangeTypeAsync(Guid taskId, Guid typeId)
    {
        var task = await GetTaskByIdAsync(taskId);
        var type = await _appDbContext.TaskTypes.FirstOrDefaultAsync(t => t.Id == typeId);
        task.Type = type;
        await UpdateAndSaveTaskAsync(task);
    }

    public async Task ChangePerformerAsync(Guid taskId, Guid? performerId)
    {
        var task = await GetTaskByIdAsync(taskId);
        task.PerformerId = performerId;
        await UpdateAndSaveTaskAsync(task);
    }

    private async Task UpdateAndSaveTaskAsync(Models.Task task)
    {
        _appDbContext.Tasks.Update(task);
        await TrySaveChangesAsync();
    }

    private async Task TrySaveChangesAsync()
    {
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO exception
            throw;
        }
    }

    private async Task<Models.Task?> GetTaskByIdAsync(Guid taskId)
    {
        return await _appDbContext.Tasks.FirstOrDefaultAsync(t=>t.Id == taskId);
    }

    public async Task<IEnumerable<TaskShortInfoDto>> GetSprintTasksAsync()
    {
        var sprintTasks = new List<TaskShortInfoDto>();
        var sprintId = await GetCurrentSprintIdAsync();
        if (sprintId is not null)
        {
            sprintTasks = await _appDbContext.Tasks
                .Where(t => t.SprintId == sprintId.Value)
                .Include(t => t.State)
                .Include(t => t.Type)
                .Select(t => new TaskShortInfoDto(t.Id, t.Title, t.State.Name, t.Type.Name, t.PerformerId, t.CreatorId))
                .ToListAsync();
        }

        return sprintTasks;
    }

    private Guid? GetRequestingIdentityId()
    {
        var identityIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(identityIdString, out var identityId)
                            ? identityId
                            : null;
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = GetRequestingIdentityId();
        if (identityId is null)
        {
            return null;
        }

        var projectsIdentity = await _appDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == identityId);

        return projectsIdentity?.ProjectId;
    }

    private async Task<TaskState> GetDefaultTaskStateAsync()
    {
        _stateCfg.BasicStates.TryGetValue(_stateCfg.DefaultState, out var basicTaskStateId);
        return await _appDbContext.TaskStates.FirstOrDefaultAsync(t => t.Id == basicTaskStateId);
    }
}
