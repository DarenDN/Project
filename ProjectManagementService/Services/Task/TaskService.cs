namespace ProjectManagementService.Services.Task;

using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Task;
using Task = System.Threading.Tasks.Task;

public sealed class TaskService : ITaskService
{
    private readonly ApplicationDbContext _appDbContext;

    public TaskService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task CreateTaskAsync(CreateTaskDto newTaskDto)
    {
        var status = await _appDbContext.TaskStatuses.FirstOrDefaultAsync(s => s.Id == newTaskDto.StatusId);
        var type = await _appDbContext.TaskTypes.FirstOrDefaultAsync(t => t.Id == newTaskDto.TypeId);

        var newTask = new Models.Task
        {
            Title = newTaskDto.Title,
            Description = newTaskDto.Description,
            DashboardId = newTaskDto.DashboardId,
            Status = status,
            Type = type
        };

        await _appDbContext.AddAsync(newTask);
        await TrySaveChangesAsync();
    }

    public async Task UpdateTaskAsync(TaskUpdateDto taskDto)
    {
        var task = await GetTaskByIdAsync(taskDto.Id);

        task.Title = taskDto.Title;
        task.Description = taskDto.Description;
        task.DashboardId = taskDto.DashboardId;
        // TODO chagne status and type
        // task.Status 
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

    public async Task<IEnumerable<TaskShortInfoDto>> GetTasksAsync(Guid dashboardId)
    {
        var tasks = await _appDbContext.Tasks
            .Where(t => t.DashboardId == dashboardId)
            .Include(t => t.Status)
            .Include(t => t.Type)
            .Select(t => new TaskShortInfoDto(t.Id, t.Title, t.Status.Name, t.Type.Name, t.PerformerId, t.CreatorId))
            .ToListAsync();

        return tasks;
    }

    public async Task<TaskDataDto> GetTaskAsync(Guid taskId)
    {
        var task = await _appDbContext.Tasks
            .Where(t => t.Id == taskId)
            .Include(t=>t.Status)
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
            task.Status.Name,
            task.Type.Name,
            task.PerformerId,
            task.CreatorId);
    }

    public async Task ChangeStatusAsync(Guid taskId, Guid statusId) 
    { 
        var task = await GetTaskByIdAsync(taskId);
        var status = await _appDbContext.TaskStatuses.FirstOrDefaultAsync(s => s.Id == statusId);
        task.Status = status;
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
}
