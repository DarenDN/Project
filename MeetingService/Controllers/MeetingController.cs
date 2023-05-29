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
            // TODO
            return BadRequest(ex.Message);
        }
    }

    public async Task<ActionResult> JoinMeetingAsync(Participant participiant, Guid projectId)
    {
        try
        {
            /* if a meeting doesnt exist, need to create one and smh store info about it
            where to store the info? how others wiil be able to join a specific meeting?
            do we need to store info bout active meeting in redis with the specific project key? each project will have a key anyway
            so in redis we will have a "Key - Meeting" pair?
             */
            /* TODO create a MeetingService with a method JoinAsync(Guid projectId)
             JoinAsync will: 
            1. Get current meeting by key - projectId (if not exists - create one)
            2. Add current user to the List of users
            3. If users > 1, signal others about new join
            4. Return MeetingDto
             */

            /* 
             creating should be in the GetMeetingAsync method, this one should only join current user to one
             */

            // user press a button and gets current state of meeting in small window with Join button
            var meeting = await _meetingService.JoinMeetingAsync(participiant, projectId);
            return Ok(meeting);
        }
        catch (Exception ex)
        {
            // TODO
            return BadRequest(ex.Message);
        }
    }

    public async Task<ActionResult> UpdateMeetingAsync(ParticipantEvaluation meeting, Guid projectId)
    {
        // TODO try catch
        await _meetingService.UpdateMeetingAsync(meeting, projectId);

        return Ok();
    }

    public async Task<ActionResult> LeaveMeetingAsync(Participant participant, Guid projectId)
    {
        try
        {
            await _meetingService.LeaveMeetingAsync(participant, projectId);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO
            return BadRequest(ex.Message);
        }
    }
}
