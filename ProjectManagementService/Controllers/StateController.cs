namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Services.State;

[ApiController, Authorize]
[EnableCors("CorsPolicy")]
[Route("api/[controller]")]
public class StateController : ControllerBase
{
    private readonly IStateService _stateService;

    public StateController(IStateService stateService)
    {
        this._stateService = stateService;
    }

    [HttpGet]
    [Route(nameof(GetStatesAsync))]
    public async Task<ActionResult> GetStatesAsync()
    {
        try
        {
            var states = await _stateService.GetStatesAsync();
            return Ok(states);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetNextStatesAsync))]
    public async Task<ActionResult> GetNextStatesAsync(Guid currentState)
    {
        try
        {
            var states = await _stateService.GetNextStatesAsync(currentState);
            return Ok(states);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
