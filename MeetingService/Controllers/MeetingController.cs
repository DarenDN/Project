namespace MeetingService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.MeetingService;
using Enums;
using Dtos;
using Microsoft.AspNetCore.Authorization;

[ApiController, Authorize]
[Route("api/[controller]")]
public sealed class MeetingController : ControllerBase
{
    private readonly IMeetingService _meetingService;

    public MeetingController(IMeetingService meetingService)
    {
        _meetingService = meetingService;
    }

    /// <summary>
    /// Returns a code of meeting if one exists
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>
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
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(JoinMeetingAsync))]
    public async Task<ActionResult> JoinMeetingAsync(string meetingCode)
    {
        try
        {
            var meetingState = await _meetingService.JoinMeetingAndNotifyAsync(meetingCode);
            return Ok(meetingState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(GetCurrentMeetingStateAsync))]
    public async Task<ActionResult> GetCurrentMeetingStateAsync(string meetingCode)
    {
        try
        {
            var meetingState = await _meetingService.GetCurrentMeetingStateAsync(meetingCode);
            return Ok(meetingState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(LeaveMeetingAsync))]
    public async Task<ActionResult> LeaveMeetingAsync(string meetingCode)
    {
        try
        {
            await _meetingService.LeaveMeetingAndNotifyAsync(meetingCode);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(CreateMeetingAndJoinAsync))]
    public async Task<ActionResult> CreateMeetingAndJoinAsync(Guid projectId, Dictionary<Guid, BacklogType> tasks)
    {
        try
        {
            var meetingCode = await _meetingService.CreateMeetingAndJoinAsync(projectId, tasks);
            return Ok(meetingCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ChangeActiveTaskAsync))]
    public async Task<ActionResult> ChangeActiveTaskAsync(string meetingCode, Guid taskId)
    {
        try
        {
            var currentTaskState = await _meetingService.ChangeActiveTaskAndNotifyAsync(meetingCode, taskId);
            return Ok(currentTaskState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(GetFinalEvaluationsAsync))]
    public async Task<ActionResult> GetFinalEvaluationsAsync(string meetingCode)
    {
        try
        {
            var results = await _meetingService.GetFinalEvaluationsAsync(meetingCode);
            return Ok(results);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ShowEvaluationsAsync))]
    public async Task<ActionResult> ShowEvaluationsAsync(string meetingCode, Guid taskId)
    {
        try
        {
            await _meetingService.ShowEvaluationsAsync(meetingCode, taskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(UpdateUserEvaluationAsync))]
    public async Task<ActionResult> UpdateUserEvaluationAsync(string meetingCode,TaskEvaluationDto evaluationDto)
    {
        try
        {
            await _meetingService.UpdateUserEvaluationAndNotifyAsync(meetingCode, evaluationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(EvaluateTaskFinalAsync))]
    public async Task<ActionResult> EvaluateTaskFinalAsync(string meetingCode,TaskEvaluationDto evaluationDto)
    {
        try
        {
            await _meetingService.EvaluateTaskFinalAndNotifyAsync(meetingCode, evaluationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(ReevaluateAsync))]
    public async Task<ActionResult> ReevaluateAsync(string meetingCode, Guid taskId)
    {
        try
        {
            await _meetingService.ReevaluateAsync(meetingCode,taskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(ChangeTaskBacklogTypeAsync))]
    public async Task<ActionResult> ChangeTaskBacklogTypeAsync(string meetingCode, Guid taskId, BacklogType backlogType)
    {
        try
        {
            await _meetingService.ChangeTaskBacklogTypeAndNotifyAsync(meetingCode,taskId, backlogType);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, AllowAnonymous]
    [Route("testGet")]
    public async Task<ActionResult> Test()
    {
        var kek = new Dictionary<Guid, BacklogType>
        {
            { Guid.NewGuid(), BacklogType.Project },
            {Guid.NewGuid(), BacklogType.Sprint },
            {Guid.NewGuid(), BacklogType.Project },
        };
        return Ok(kek);
    }

    [HttpPut, AllowAnonymous]
    [Route("testPut")]
    public async Task<ActionResult> Test(Dictionary<Guid, BacklogType> keyValuePairs)
    {
        return Ok();
    }
}
