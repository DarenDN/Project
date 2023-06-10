namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos.Sprint;
using Services.Sprint;

[Route("api/[controller]"), Authorize]
[ApiController]
public sealed class SprintController : ControllerBase
{
    private readonly ISprintService _sprintService;

    public SprintController(ISprintService sprintService)
    {
        _sprintService = sprintService;
    }

    [HttpPost]
    [Route(nameof(CreateSprintAsync))]
    public async Task<ActionResult> CreateSprintAsync(CreateSprintDto createSprintDto)
    {
        try
        {
            await _sprintService.CreateSprintAsync(createSprintDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetCurrentSprintAsync))]
    public async Task<ActionResult> GetCurrentSprintAsync()
    {
        try
        {
            var sprint = await _sprintService.GetCurrentSprintDtoAsync();
            return Ok(sprint);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetSprintAsync))]
    public async Task<ActionResult> GetSprintAsync(Guid sprintId)
    {
        try
        {
            var sprint = await _sprintService.GetSprintAsync(sprintId);
            return Ok(sprint);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
