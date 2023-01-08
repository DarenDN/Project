namespace ProjectManagementService.Controllers;
using Microsoft.AspNetCore.Mvc;
using Autofac;
using Dtos;
using Handlers;
using Data;

[ApiController]
[Route("api/[controller]")]
internal sealed class DashboardController : ControllerBase
{
    [HttpPost()]
    [Route(nameof(GetDashboardsByUser))]
    // TODO pass in user ID, JWT
    public async Task<JsonResult> GetDashboardsByUser(ProjectDto projectDto /* TODO user ID */)
    {
        var dashboards = await GetDashboardsByUser(projectDto.Id);
        return new JsonResult(dashboards);
    }

    // TODO we need also a Dashboard ID here
    [HttpPost()]
    [Route(nameof(GetDashboardTasks))]
    public async Task<JsonResult> GetDashboardTasks(DashboardDto dashboardDto)
    {
        var tasks = await GetDashboardTasks(dashboardDto.ID);
        return new JsonResult(tasks);
    }

    private async Task<IEnumerable<TaskDto>> GetDashboardTasks(Guid dashboardId)
    {
        var appDbContext = Application.Container.Resolve<ApplicationDbContext>();
        // TODO store the handler in the container
        var taskHandler = new TaskHandler(appDbContext);
        return await taskHandler.GetDashboardTasks(dashboardId);
    }

    // TODO we need user ID, JWT prob
    private async Task<IEnumerable<DashboardDto>> GetDashboardsByUser(Guid projectId)
    {
        var appDbContext = Application.Container.Resolve<ApplicationDbContext>();
        // TODO store the handler in the container
        var dashboardHandler = new DashboardHandler(appDbContext);
        return await dashboardHandler.GetProjectDashboardsByUserId(projectId);
    }
}
