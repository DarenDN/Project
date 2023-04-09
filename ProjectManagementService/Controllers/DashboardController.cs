namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos;
using Services;

[ApiController]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
    private DashboardService _dashboardService;

    public DashboardController(DashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpPut()]
    [Route(nameof(UpdateDashboardAsync))]
    // TODO pass in user Id, JWT
    public async Task<JsonResult> UpdateDashboardAsync(DashboardDto dashboardDto)
    {
        var createdDashboardDto = await _dashboardService.UpdateDashboardAsync(dashboardDto);
        
        return new JsonResult(createdDashboardDto);
    }

    [HttpPost()]
    [Route(nameof(CreateDashboardAsync))]
    // TODO pass in user Id, JWT
    public async Task<JsonResult> CreateDashboardAsync(DashboardDto dashboardDto)
    {
        var createdDashboardDto = await _dashboardService.CreateDashboardAsync(dashboardDto);
        
        return new JsonResult(createdDashboardDto);
    }

    [HttpDelete()]
    [Route(nameof(DeleteDashboardAsync))]
    // TODO pass in user Id, JWT
    public async Task<JsonResult> DeleteDashboardAsync(Guid dashboardId)
    {
        var createdDashboardDto = await _dashboardService.DeleteDashboardAsync(dashboardId);
        
        return new JsonResult(createdDashboardDto);
    }

    // TODO we need also a Dashboard Id here
    [HttpGet()]
    [Route(nameof(GetDashboardTasksAsync))]
    public async Task<JsonResult> GetDashboardTasksAsync(Guid dashboardId)
    {
        var tasks = await _dashboardService.GetTasksAsync(dashboardId);
        return new JsonResult(tasks);
    }
}
