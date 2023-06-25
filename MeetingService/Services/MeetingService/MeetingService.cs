namespace MeetingService.Services.MeetingService;

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Hubs;
using CacheService;
using Dtos;
using Enums;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public sealed class MeetingService : IMeetingService
{
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccesor;
    private readonly IHubContext<MeetingHub, IMeetingHub> _meetingHubContext;
    public MeetingService(
        ICacheService cacheService,
        IHubContext<MeetingHub, IMeetingHub> meetingHubContext,
        IHttpContextAccessor httpContextAccesor)
    {
        _cacheService = cacheService;
        _meetingHubContext = meetingHubContext;
        _httpContextAccesor = httpContextAccesor;
    }

    public async Task<MeetingStateDto> JoinMeetingAndNotifyAsync( string userName)
    {
        var meetingCode = GetRequestingMeetingCode();
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.AddCasheUserConnectionAsync(meetingCode, connectionId, userId.Value, userName);
        var userCachedEvaluations = await _cacheService.GetUserCachedEvaluationsAsync(meetingCode, userId.Value);
        await _meetingHubContext.Groups.AddToGroupAsync(connectionId, meetingCode);
        await _meetingHubContext.Clients.All
            .UserJoinedMeetingAsync(userCachedEvaluations);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        return meetingState;
    }

    public async Task<MeetingStateDto> GetCurrentMeetingStateAsync()
    {
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(GetRequestingMeetingCode());
        return meetingState;
    }

    public async Task UpdateUserEvaluationAndNotifyAsync( TaskEvaluationDto evaluationDto)
    {
        var meetingCode = GetRequestingMeetingCode();
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.SetEvaluationAsync(meetingCode, userId.Value, evaluationDto);
        var participantEvaluationDto = await _cacheService.GetUserCachedEvaluationsAsync (meetingCode, userId.Value);
        await _meetingHubContext.Clients.All.UpdateUserEvaluationAsync(participantEvaluationDto);
    }

    public async Task LeaveMeetingAndNotifyAsync()
    {
        var meetingCode = GetRequestingMeetingCode();
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.RemoveCasheUserConnectionAsync(meetingCode, userId.Value);
        await _meetingHubContext.Groups.RemoveFromGroupAsync(connectionId, meetingCode);
        await _meetingHubContext.Clients.All
            .UserLeavedMeetingAsync(new ParticipantIdDto(userId.Value));
    }

    public async Task DeleteMeetingAsync()
    {
        var meetingCode = GetRequestingMeetingCode();
        var connections = await _cacheService.GetCasheUserConnectionAsync (meetingCode);
        await _cacheService.DeleteCasheMeetingAsync(meetingCode);
        await _meetingHubContext.Clients.All.DeleteMeetingAsync();
        foreach (var connection in connections)
        {
            await _meetingHubContext.Groups.RemoveFromGroupAsync(connection, meetingCode);
        }
    }

    public async Task<bool> IsMeetingExistAsync()
    {
        var meetingCode = GetRequestingMeetingCode();
        var meetingExists = await _cacheService.IsMeetingExistAsync(meetingCode);
        return meetingExists;
    }

    public async Task<MeetingStateDto> CreateMeetingAndJoinAsync(string userName, Guid projectId, IEnumerable<BacklogTaskDto> tasks)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        var meetingCode = await _cacheService.CreateCacheMeetingAsync( projectId, tasks);
        await _cacheService.AddCasheUserConnectionAsync(meetingCode, connectionId, userId.Value, userName);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        await _meetingHubContext.Groups.AddToGroupAsync(connectionId,meetingCode);
        return meetingState;
    }

    public async Task<CurrentTaskStateDto> ChangeActiveTaskAndNotifyAsync( Guid taskId)
    {
        var meetingCode = GetRequestingMeetingCode();
        var currentTaskDto = await _cacheService.SetActiveTaskAsync(meetingCode, taskId);
        await _meetingHubContext.Clients.All
            .ChangeActiveTaskAsync(currentTaskDto);
        return currentTaskDto;
    }

    public async Task ShowEvaluationsAsync( Guid taskId)
    {
        var meetingCode = GetRequestingMeetingCode();
        await _cacheService.SetEvaluationsOpenAsync(meetingCode, taskId);
        await _meetingHubContext.Clients.All
            .ShowEvaluationsAsync(new TaskDto(taskId));
    }

    public async Task ReevaluateAsync( Guid taskId)
    {
        var meetingCode = GetRequestingMeetingCode();
        var currentTaskState = await _cacheService.DeleteTaskEvaluationsAsync(meetingCode, taskId);

        await _meetingHubContext.Clients.All
            .ReevaluateAsync(currentTaskState);
    }

    public async Task EvaluateTaskFinalAndNotifyAsync( TaskEvaluationDto evaluationDto)
    {
        var meetingCode = GetRequestingMeetingCode();
        await _cacheService.SetEvaluationFinalAsync(meetingCode, evaluationDto);
        await _meetingHubContext.Clients.All.EvaluateTaskFinalAsync(evaluationDto);
    }

    public async Task ChangeTaskBacklogTypeAndNotifyAsync( Guid taskId, BacklogType backlogType)
    {
        var meetingCode = GetRequestingMeetingCode();
        await _cacheService.ChangeTaskBacklogTypeAsync(meetingCode, taskId, backlogType);
        await _meetingHubContext.Clients.All
            .ChangeTaskBacklogTypeAsync(new TaskBacklogChangedDto(taskId, backlogType));

    }

    public async Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync()
    {
        var meetingCode = GetRequestingMeetingCode();
        var finalTaskStatesAsync = await _cacheService.GetFinalEvaluationsAsync(meetingCode);
        return finalTaskStatesAsync;
    }

    private string GetRequestingConnectionId()
    {
        return _httpContextAccesor.HttpContext.Connection.Id;
    }

    private Guid? GetRequestingUserId()
    {
        var userIdString = _httpContextAccesor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(userIdString, out var userId) ?userId:null;
    }

    private string GetRequestingMeetingCode()
    {
        const string MeetingCodeHeaderKey = "MeetingCode";
        _httpContextAccesor.HttpContext.Request.Headers.TryGetValue(MeetingCodeHeaderKey, out var meetingCode);
        return meetingCode;
    }

    public async Task TestSignalR()
    {
        await _meetingHubContext.Clients.All.TestHub("Test string");
    }
}
