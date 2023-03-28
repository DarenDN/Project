namespace ProjectManagementService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using Dtos;
using Services;
using Data;

[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
    private readonly ApplicationDbContext _appDbContext;

    public DashboardController(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    [HttpGet()]
    [Route(nameof(GetDashboardsByUser))]
    // TODO pass in user ID, JWT
    public async Task<JsonResult> GetDashboardsByUser(ProjectDto projectDto /* TODO user ID */)
    {
        var dashboards = await GetDashboardsByUser(projectDto.Id);
        return new JsonResult(dashboards);
    }

    // TODO we need also a Dashboard ID here
    [HttpGet()]
    [Route(nameof(GetDashboardTasks))]
    public async Task<JsonResult> GetDashboardTasks(DashboardDto dashboardDto)
    {
        var tasks = await GetDashboardTasks(dashboardDto.ID);
        return new JsonResult(tasks);
    }

    private async Task<IEnumerable<TaskDto>> GetDashboardTasks(Guid dashboardId)
    {
        // TODO store the handler in the container
        var taskHandler = new TaskService(_appDbContext);
        return await taskHandler.GetDashboardTasks(dashboardId);
    }

    // TODO we need user ID, JWT prob
    private async Task<IEnumerable<DashboardDto>> GetDashboardsByUser(Guid projectId)
    {
        // TODO store the handler in the container
        var dashboardHandler = new DashboardService(_appDbContext);
        return await dashboardHandler.GetProjectDashboardsByUserId(projectId);
    }
}
