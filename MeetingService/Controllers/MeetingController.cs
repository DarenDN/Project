namespace MeetingService.Controllers;

using MeetingService.Services.MeetingService;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services.Interfaces;

[ApiController]
[Route("api/[controller]")]
public sealed class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;

    public MeetingController( IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    [HttpGet]
    [Route(nameof(GetMeetingAsync))]
    public async Task<ActionResult> GetMeetingAsync(Guid projectId)
    {
        try
        {
            var meeting = await _meetingService.GetMeetingAsync(projectId);
            return Ok(meeting);
        }
        catch (Exception ex)
        {
            return StatusCode(500,ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(JoinMeetingAsync))]
    public async Task<ActionResult> JoinMeetingAsync(string meetingCode)
    {
        try
        {
            var meeting = await _meetingService.JoinMeetingAsync(meetingCode);
            return Ok(meeting);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
