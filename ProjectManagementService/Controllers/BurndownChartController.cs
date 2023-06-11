namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ProjectManagementService.Services.BurndownChart;

[Route("api/[controller]"), Authorize]
[EnableCors("CorsPolicy")]
[ApiController]
public sealed class BurndownChartController : ControllerBase
{
    private readonly IBurndownChartService _burndownChartService;

    public BurndownChartController(IBurndownChartService burndownChartService)
    {
        _burndownChartService = burndownChartService;
    }
}
