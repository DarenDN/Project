namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.Dashboard;
using Dtos.Dashboard;

[ApiController, Authorize]
[Route("api/[controller]")]
public sealed class DashboardController : ControllerBase
{
    private IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpPut]
    [Route(nameof(UpdateDashboardAsync))]
    public async Task<ActionResult> UpdateDashboardAsync(UpdateDashboardDto dashboardDto)
    {
        try
        {
            var createdDashboardDto = await _dashboardService.UpdateDashboardAsync(dashboardDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }   
    }

    [HttpPost]
    [Route(nameof(CreateDashboardAsync))]
    public async Task<ActionResult> CreateDashboardAsync(CreateDashboardDto dashboardDto)
    {
        try
        {
            var createdDashboardDto = await _dashboardService.CreateDashboardAsync(dashboardDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route(nameof(DeleteDashboardAsync))]
    public async Task<ActionResult> DeleteDashboardAsync(Guid dashboardId)
    {
        try
        {
            await _dashboardService.DeleteDashboardAsync(dashboardId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetDashboardsAsync))]
    public async Task<ActionResult> GetDashboardsAsync()
    {
        try
        {
            var dashboards = await _dashboardService.GetDashboardsAsync();
            return Ok(dashboards);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetDashboardAsync))]
    public async Task<ActionResult> GetDashboardAsync(Guid dashboardId)
    {
        try
        {
            var dashboard = await _dashboardService.GetDashboardAsync(dashboardId);
            return Ok(dashboard);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
