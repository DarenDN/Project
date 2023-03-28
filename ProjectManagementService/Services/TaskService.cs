namespace ProjectManagementService.Services;
using Data;
using Dtos;
public sealed class TaskService
{
    private readonly ApplicationDbContext _applicationDbContext;
    public TaskService(ApplicationDbContext appDbContext)
    {
        _applicationDbContext = appDbContext;
    }

    public async Task<IEnumerable<TaskDto>> GetDashboardTasks(Guid dashboardId)
    {
        var tasks = _applicationDbContext.TaskModels.Where(d => d.Dashboard.ID == dashboardId);
        var taskDtos = tasks.Select(d => new TaskDto(d));
        return taskDtos;
    }
}

