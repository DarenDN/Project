namespace MeetingService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Services.MeetingService;
using Enums;
using Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

[ApiController, Authorize]
[EnableCors("CorsPolicy")]
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
            var meetingCode = await _meetingService.GetMeetingAsync(projectId);
            return new JsonResult(new { meetingCode });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(JoinMeetingAsync))]
    public async Task<ActionResult> JoinMeetingAsync(string userName)
    {
        try
        {
            var meetingState = await _meetingService.JoinMeetingAndNotifyAsync(userName);
            return new JsonResult(meetingState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(GetCurrentMeetingStateAsync))]
    public async Task<ActionResult> GetCurrentMeetingStateAsync()
    {
        try
        {
            var meetingState = await _meetingService.GetCurrentMeetingStateAsync();
            return new JsonResult(meetingState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(LeaveMeetingAsync))]
    public async Task<ActionResult> LeaveMeetingAsync()
    {
        try
        {
            await _meetingService.LeaveMeetingAndNotifyAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(DeleteMeetingAsync))]
    public async Task<ActionResult> DeleteMeetingAsync()
    {
        try
        {
            await _meetingService.DeleteMeetingAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(CreateMeetingAndJoinAsync))]
    public async Task<ActionResult> CreateMeetingAndJoinAsync(CreateMeetingDto createMeetingDto)
    {
        try
        {
            var meetingCode = await _meetingService.CreateMeetingAndJoinAsync(createMeetingDto.userName, createMeetingDto.projectId, createMeetingDto.tasks);
            return new JsonResult(meetingCode);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ChangeActiveTaskAsync))]
    public async Task<ActionResult> ChangeActiveTaskAsync( TaskDto taskDto)
    {
        try
        {
            var currentTaskState = await _meetingService.ChangeActiveTaskAndNotifyAsync(taskDto.TaskId);
            return new JsonResult(currentTaskState);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(GetFinalEvaluationsAsync))]
    public async Task<ActionResult> GetFinalEvaluationsAsync()
    {
        try
        {
            var backlogTasksDto = await _meetingService.GetFinalEvaluationsAsync();
            return new JsonResult(new { backlogTasksDto });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut]
    [Route(nameof(ShowEvaluationsAsync))]
    public async Task<ActionResult> ShowEvaluationsAsync(TaskDto taskDto)
    {
        try
        {
            await _meetingService.ShowEvaluationsAsync(taskDto.TaskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(UpdateUserEvaluationAsync))]
    public async Task<ActionResult> UpdateUserEvaluationAsync(TaskEvaluationDto evaluationDto)
    {
        try
        {
            await _meetingService.UpdateUserEvaluationAndNotifyAsync(evaluationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(EvaluateTaskFinalAsync))]
    public async Task<ActionResult> EvaluateTaskFinalAsync(TaskEvaluationDto evaluationDto)
    {
        try
        {
            await _meetingService.EvaluateTaskFinalAndNotifyAsync(evaluationDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(ReevaluateAsync))]
    public async Task<ActionResult> ReevaluateAsync(TaskDto taskDto)
    {
        try
        {
            await _meetingService.ReevaluateAsync(taskDto.TaskId);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(ChangeTaskBacklogTypeAsync))]
    public async Task<ActionResult> ChangeTaskBacklogTypeAsync(Guid taskId, BacklogType backlogType)
    {
        try
        {
            await _meetingService.ChangeTaskBacklogTypeAndNotifyAsync(taskId, backlogType);
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

    [HttpPut, AllowAnonymous]
    [Route("testSignalR")]
    public async Task<ActionResult> SignalR()
    {
        await _meetingService.TestSignalR();
        return Ok();
    }
}
