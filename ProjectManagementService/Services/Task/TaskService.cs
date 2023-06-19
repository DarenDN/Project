namespace ProjectManagementService.Services.Task;

using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Task;
using Task = System.Threading.Tasks.Task;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Configurations;
using Models;
using ProjectManagementService.Dtos.Estimation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ProjectManagementService.Dtos.Backlog;
using ProjectManagementService.Services.State;

public sealed class TaskService : ITaskService
{
    private readonly Data.AppDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IStateService _stateService;
    private readonly StateConfiguration _stateCfg;

    public TaskService(
        Data.AppDbContext appDbContext, 
        IHttpContextAccessor httpContextAccessor, 
        IStateService stateService,
        IOptions<StateConfiguration> options)
    {
        _appDbContext = appDbContext;
        this._httpContextAccessor = httpContextAccessor;
        this._stateService = stateService;
        _stateCfg = options.Value;
    }

    public async Task CreateTaskAsync(CreateTaskDto newTaskDto)
    {
        var type = await _appDbContext.TaskTypes.FirstOrDefaultAsync(t => t.Id == newTaskDto.TypeId);
        var state = await GetDefaultTaskStateAsync();
        var creator = GetRequestingIdentityId();
        var projectId = await GetRequestingUsersProjectIdAsync(creator);
        var newTask = new Models.Task
        {
            Title = newTaskDto.Title,
            Description = newTaskDto.Description,
            State = state,
            Type = type,
            CreatorId = creator.Value,
            ProjectId = projectId.Value
        };

        if (newTaskDto.SprintId.HasValue)
        {
            newTask.SprintId = newTaskDto.SprintId.Value;
        }

        await _appDbContext.Tasks.AddAsync(newTask);
        await TrySaveChangesAsync();
    }

    public async Task<IEnumerable<BacklogTaskDto>> GetTasksBacklogAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var tasks = await _appDbContext.Tasks.Where(t=>t.ProjectId == projectId.Value).ToListAsync();
        var currentSprintId = await GetCurrentSprintIdAsync();
        var backlogTasks = tasks.Select(t=>new BacklogTaskDto(
            t.Id,
            t.EstimationInTime, 
            t.EstimationInPoints,
            t.SprintId.HasValue && currentSprintId.HasValue && t.SprintId == currentSprintId
                    ? 1
                    : 0));
        return backlogTasks;
    }

    public async Task UpdateTasksAsync(IEnumerable<BacklogTaskDto> taskSprintInfos)
    {
        var currentSprintId = await GetCurrentSprintIdAsync();
        var updatedTasks = new List<Models.Task>();
        foreach(var taskSprintInfo in taskSprintInfos)
        {
            var task = await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == taskSprintInfo.TaskId);
            if(task is null )
            {
                continue;
            }

            if(taskSprintInfo.BacklogType == 1)
            {
                task.SprintId = currentSprintId;
                task.EstimationInPoints = taskSprintInfo.EstimationPoint;
                task.EstimationInTime = taskSprintInfo.EstimationTime;
            }
            else
            {
                task.SprintId = null;
                task.EstimationInPoints = taskSprintInfo.EstimationPoint;
                task.EstimationInTime = taskSprintInfo.EstimationTime;
            }
            var toWorkState = await GetToWorkStateAsync();
            if (task.State.Order <= toWorkState.Order)
            {
                task.State = taskSprintInfo.EstimationPoint != null || taskSprintInfo.EstimationTime != null
                    ? await GetToWorkStateAsync()
                    : await GetDefaultTaskStateAsync();
            }

            updatedTasks.Add(task);
        }

        _appDbContext.Tasks.UpdateRange(updatedTasks);

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
        var ptojectId = await GetRequestingUsersProjectIdAsync();
        var tasks = await _appDbContext.Tasks
            .Where(t => t.ProjectId == ptojectId)
            .Include(t => t.State)
            .Include(t => t.Type)
            .Select(t => new TaskShortInfoDto(t.Id, t.Title, t.State.Name, t.Type.Name, t.PerformerId, t.CreatorId))
            .ToListAsync();

        return tasks;
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
        var nextStates = await _stateService.GetNextStatesAsync(task.State.Id);
        return new TaskDataDto(
            task.Id, 
            task.Title, 
            task.Description,
            task.State.Name,
            task.Type.Name,
            task.EstimationInTime,
            task.EstimationInPoints,
            nextStates,
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

    public async Task SetEstimationManyAsync(List<EstimationDto> estimationDtos)
    {
        var estimatedTasks = new List<Models.Task>();
        foreach(var estimation in estimationDtos)
        {
            var task = await _appDbContext.Tasks.FirstOrDefaultAsync(t => t.Id == estimation.TaskId);
            task.EstimationInPoints = estimation.EstimationInPoints;
            task.EstimationInTime = estimation.EstimationInTime;
            estimatedTasks.Add(task);
        }
        _appDbContext.Tasks.UpdateRange(estimatedTasks);
        await TrySaveChangesAsync();
    }

    public async Task SetEstimationSingleAsync(EstimationDto estimationDto)
    {
        var task = await GetTaskByIdAsync(estimationDto.TaskId);
        task.EstimationInPoints = estimationDto.EstimationInPoints;
        task.EstimationInTime = estimationDto.EstimationInTime;
        _appDbContext.Tasks.Update(task);
        await TrySaveChangesAsync();
    }

    public async Task SetStoryManyAsync(List<Guid> taskIds)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var currentSprint = await GetCurrentSprintIdAsync(projectId);
        var tasks = await _appDbContext.Tasks.Where(t => taskIds.Contains(t.Id)).ToListAsync();
        foreach(var task in tasks)
        {
            task.SprintId = currentSprint;
        }
        _appDbContext.Tasks.UpdateRange(tasks);
        await TrySaveChangesAsync();
    }

    public async Task SetStorySingleAsync(Guid taskId)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var currentSprint = await GetCurrentSprintIdAsync(projectId);
        var task = await GetTaskByIdAsync(taskId);
        task.SprintId = currentSprint;
        _appDbContext.Tasks.Update(task);
        await TrySaveChangesAsync();
    }

    public async Task<IEnumerable<TaskShortInfoDto>> GetTasksWithStatesAsync(List<Guid>? states)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var tasksQuery = _appDbContext.Tasks
                .Include(t => t.State)
                .Include(t => t.Type)
                .Where(t => t.ProjectId == projectId);

        if(states is not null && states.Any())
        {
            tasksQuery = tasksQuery.Where(t=> states.Contains(t.State.Id));
        }

        return await tasksQuery
                .Select(t => new TaskShortInfoDto(t.Id, t.Title, t.State.Name, t.Type.Name, t.PerformerId, t.CreatorId))
                .ToListAsync();
    }

    private async Task<Guid?> GetCurrentSprintIdAsync(Guid? projectId = null)
    {
        projectId ??= await GetRequestingUsersProjectIdAsync();
        var today = DateTime.Now;
        var currentSprint = await _appDbContext.Sprints
            .FirstOrDefaultAsync(s => s.ProjectId == projectId
                                             && s.DateStart <= today
                                             && s.DateEnd >= today);
        return currentSprint?.Id;
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

    private Guid? GetRequestingIdentityId()
    {
        var identityIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(identityIdString, out var identityId)
                            ? identityId
                            : null;
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync(Guid? identityId = null)
    {
        identityId ??= GetRequestingIdentityId();
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

    private async Task<TaskState> GetToWorkStateAsync()
    {
        _stateCfg.BasicStates.TryGetValue(_stateCfg.ToWorkState, out var toWorkState);
        return await _appDbContext.TaskStates.FirstOrDefaultAsync(t => t.Id == toWorkState);
    }

    private async Task<TaskState> GetEvaluationStateAsync()
    {
        _stateCfg.BasicStates.TryGetValue(_stateCfg.EvaluationState, out var evaluationState);
        return await _appDbContext.TaskStates.FirstOrDefaultAsync(t => t.Id == evaluationState);
    }

    public async Task SetCurrentUserAsPerformerAsync(Guid taskId)
    {
        var currentUserId = GetRequestingIdentityId();
        await ChangePerformerAsync(taskId, currentUserId);
    }
}
