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
        var meetingCode = GetReauestingMeetingCode();
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.AddCasheUserConnectionAsync(meetingCode, connectionId, userId.Value, userName);
        var userCachedEvaluations = await _cacheService.GetUserCachedEvaluationsAsync(meetingCode, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                connectionId)
            .UserJoinedMeetingAsync(userCachedEvaluations);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        return meetingState;
    }

    public async Task<MeetingStateDto> GetCurrentMeetingStateAsync()
    {
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(GetReauestingMeetingCode());
        return meetingState;
    }

    public async Task UpdateUserEvaluationAndNotifyAsync( TaskEvaluationDto evaluationDto)
    {
        var meetingCode = GetReauestingMeetingCode();
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.SetEvaluationAsync(meetingCode, userId.Value, evaluationDto);
        var participantEvaluationDto = await _cacheService.GetUserCachedEvaluationsAsync (meetingCode, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                connectionId)
            .UpdateUserEvaluationAsync(participantEvaluationDto);
    }

    public async Task LeaveMeetingAndNotifyAsync()
    {
        var meetingCode = GetReauestingMeetingCode();
        var userId = GetRequestingUserId();
        await _cacheService.RemoveCasheUserConnectionAsync(meetingCode, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .UserLeavedMeetingAsync(userId.Value);
    }

    public async Task DeleteMeetingAsync()
    {
        var meetingCode = GetReauestingMeetingCode();
        await _cacheService.DeleteCasheMeetingAsync(meetingCode);
    }

    public async Task<string> GetMeetingAsync(Guid projectId)
    {
        var meetingCode = await _cacheService.GetMeetingCodeOrNullAsync(projectId);
        return meetingCode;
    }

    public async Task<MeetingStateDto> CreateMeetingAndJoinAsync(string userName, Guid projectId, IEnumerable<BacklogTaskDto> tasks)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        var meetingCode = await _cacheService.CreateCacheMeetingAsync( projectId, tasks);
        await _cacheService.AddCasheUserConnectionAsync(meetingCode, connectionId, userId.Value, userName);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        return meetingState;
    }

    public async Task<CurrentTaskStateDto> ChangeActiveTaskAndNotifyAsync( Guid taskId)
    {
        var meetingCode = GetReauestingMeetingCode();
        var currentTaskDto = await _cacheService.SetActiveTaskAsync(meetingCode, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ChangeActiveTaskAsync(currentTaskDto);
        return currentTaskDto;
    }

    public async Task ShowEvaluationsAsync( Guid taskId)
    {
        var meetingCode = GetReauestingMeetingCode();
        await _cacheService.SetEvaluationsOpenAsync(meetingCode, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ShowEvaluationsAsync(taskId);
    }

    public async Task ReevaluateAsync( Guid taskId)
    {
        var meetingCode = GetReauestingMeetingCode();
        var currentTaskState = await _cacheService.DeleteTaskEvaluationsAsync(meetingCode, taskId);

        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ReevaluateAsync(currentTaskState);
    }

    public async Task EvaluateTaskFinalAndNotifyAsync( TaskEvaluationDto evaluationDto)
    {
        var meetingCode = GetReauestingMeetingCode();
        await _cacheService.SetEvaluationFinalAsync(meetingCode, evaluationDto);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .EvaluateTaskFinalAsync(evaluationDto);
    }

    public async Task ChangeTaskBacklogTypeAndNotifyAsync( Guid taskId, BacklogType backlogType)
    {
        var meetingCode = GetReauestingMeetingCode();
        await _cacheService.ChangeTaskBacklogTypeAsync(meetingCode, taskId, backlogType);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ChangeTaskBacklogTypeAsync(taskId, backlogType);

    }

    public async Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync()
    {
        var meetingCode = GetReauestingMeetingCode();
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

    private string GetReauestingMeetingCode()
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
