namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos.Sprint;
using Services.Sprint;
using Microsoft.AspNetCore.Cors;

[Route("api/[controller]"), Authorize]
[EnableCors("CorsPolicy")]
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
            return new JsonResult(sprint);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(FinishSprintAsync))]
    public async Task<ActionResult> FinishSprintAsync()
    {
        try
        {
            await _sprintService.FinishSprintAsync();
            return Ok();
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
            return new JsonResult(sprint);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
