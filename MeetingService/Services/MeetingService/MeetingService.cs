namespace MeetingService.Services.MeetingService;

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Security.Claims;
using Hubs;
using Dtos;
using CacheService;

public sealed class MeetingService
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

    public async Task ChangeActiveTaskAsync(Guid taskId, string meetingCode)
    {
        var currentTaskDto = await _cacheService.SetActiveTaskAsync(meetingCode, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ChangeActiveTaskAsync(currentTaskDto);
    }

    public async Task ShowEvaluationsAsync(string group, Guid taskId)
    {
        await _cacheService.SetTaskOpenedAsync(group, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                GetRequestingConnectionId())
            .ShowEvaluationsAsync(taskId);
    }

    public async Task ReevaluateAsync(string group, Guid taskId)
    {
        await _cacheService.DeleteTaskEvaluationsAsync(group, taskId);
        // TODO TaskStateChangedAsync
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                GetRequestingConnectionId())
            .ReevaluateAsync(taskId);
    }

    public async Task UpdateEvaluationAsync(string group, TaskEvaluationDto evaluationDto)
    {
        var userId = GetRequestingUserId();
        // TODO TaskStateChangedAsync
        var currentTaskStateDto = await _cacheService.SetEvaluationAsync(group, userId.Value, evaluationDto);
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                GetRequestingConnectionId())
            .EvaluateTaskAsync(currentTaskStateDto);
    }

    public async Task EvaluateTaskFinalAsync(string group, TaskEvaluationDto evaluationDto)
    {
        var currentTaskStateDto = await _cacheService.SetEvaluationFinalAsync(group, evaluationDto);
        // TODO TaskStateChangedAsync
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                GetRequestingConnectionId())
            .EvaluateTaskFinalAsync(currentTaskStateDto);
    }

    public async Task RemoveFromSprintBacklogAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }

    public async Task AddToSprintBacklogAsync(Guid taskId)
    {
        throw new NotImplementedException();
    }

    public async Task JoinMeetingAsync(string group)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.AddCasheUserConnectionAsync(group, connectionId, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                connectionId)
            .JoinMeetingAsync(userId.Value);
    }

    public async Task LeaveMeetingAsync(string group)
    {
        var userId = GetRequestingUserId();
        await _cacheService.RemoveCasheUserConnectionAsync(group, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                group,
                GetRequestingConnectionId())
            .LeaveMeetingAsync(userId.Value);
    }

    public async Task DeleteMeetingAsync(Guid projectId)
    {
        throw new NotImplementedException();
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
}
