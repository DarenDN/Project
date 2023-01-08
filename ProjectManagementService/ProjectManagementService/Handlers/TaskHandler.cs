namespace ProjectManagementService.Handlers;
using Data;
using Dtos;
internal sealed class TaskHandler
{
    private readonly ApplicationDbContext _applicationDbContext;
    public TaskHandler(ApplicationDbContext appDbContext)
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

