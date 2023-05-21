namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos.Sprint;

[Route("api/[controller]"), Authorize]
[ApiController]
public sealed class SprintController : ControllerBase
{
    private readonly ISprintSerivce _sprintSerivce;

    public SprintController(ISprintSerivce sprintSerivce)
    {
        _sprintSerivce = sprintSerivce;
    }

    [HttpPost]
    [Route(nameof(CreateSprintAsync))]
    public async Task<ActionResult> CreateSprintAsync(CreateSprintDto createSprintDto)
    {
        return Ok();
    }
}
