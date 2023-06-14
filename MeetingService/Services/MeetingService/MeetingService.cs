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

    public async Task<MeetingStateDto> JoinMeetingAndNotifyAsync(string meetingCode)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.AddCasheUserConnectionAsync(meetingCode, connectionId, userId.Value);
        var userCachedEvaluations = await _cacheService.GetUserCachedEvaluationsAsync(meetingCode, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                connectionId)
            .UserJoinedMeetingAsync(userCachedEvaluations);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        return meetingState;
    }

    public async Task<MeetingStateDto> GetCurrentMeetingStateAsync(string meetingCode)
    {
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(meetingCode);
        return meetingState;
    }

    public async Task UpdateUserEvaluationAndNotifyAsync(string meetingCode, TaskEvaluationDto evaluationDto)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        await _cacheService.SetEvaluationAsync(meetingCode, userId.Value, evaluationDto);
        var participantEvaluationDto = new ParticipantEvaluationDto(userId.Value, new EvaluationDto(evaluationDto.EvaluationPoints, evaluationDto.EvaluationTime));
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                connectionId)
            .UpdateUserEvaluationAsync(participantEvaluationDto);
    }

    public async Task LeaveMeetingAndNotifyAsync(string meetingCode)
    {
        var userId = GetRequestingUserId();
        await _cacheService.RemoveCasheUserConnectionAsync(meetingCode, userId.Value);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .UserLeavedMeetingAsync(userId.Value);
    }

    public async Task DeleteMeetingAsync(string meetingCode, Guid projectId)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetMeetingAsync(Guid projectId)
    {
        var meetingCode = await _cacheService.GetMeetingCodeOrNullAsync(projectId);
        return meetingCode;
    }

    public async Task<MeetingStateDto> CreateMeetingAndJoinAsync(Guid projectId, IEnumerable<BacklogTaskDto> tasks)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        var code = await _cacheService.CreateCacheMeetingAsync(projectId, tasks);
        await _cacheService.AddCasheUserConnectionAsync(code, connectionId, userId.Value);
        var meetingState = await _cacheService.GetCacheMeetingStateAsync(code);
        return meetingState;
    }

    public async Task<string> CreateMeetingAndJoinAsync(Guid projectId, Dictionary<Guid, BacklogType> tasks)
    {
        var userId = GetRequestingUserId();
        var connectionId = GetRequestingConnectionId();
        var code = await _cacheService.CreateCacheMeetingAsync(projectId, tasks);
        await _cacheService.AddCasheUserConnectionAsync(code, connectionId, userId.Value);
        return code;
    }

    public async Task<CurrentTaskStateDto> ChangeActiveTaskAndNotifyAsync(string meetingCode, Guid taskId)
    {
        var currentTaskDto = await _cacheService.SetActiveTaskAsync(meetingCode, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ChangeActiveTaskAsync(currentTaskDto);
        return currentTaskDto;
    }

    public async Task ShowEvaluationsAsync(string meetingCode, Guid taskId)
    {
        await _cacheService.SetEvaluationsOpenAsync(meetingCode, taskId);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ShowEvaluationsAsync(taskId);
    }

    public async Task ReevaluateAsync(string meetingCode, Guid taskId)
    {
        await _cacheService.DeleteTaskEvaluationsAsync(meetingCode, taskId);

        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ReevaluateAsync(taskId);
    }

    public async Task EvaluateTaskFinalAndNotifyAsync(string meetingCode, TaskEvaluationDto evaluationDto)
    {
        await _cacheService.SetEvaluationFinalAsync(meetingCode, evaluationDto);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .EvaluateTaskFinalAsync(evaluationDto);
    }

    public async Task ChangeTaskBacklogTypeAndNotifyAsync(string meetingCode, Guid taskId, BacklogType backlogType)
    {
        await _cacheService.ChangeTaskBacklogTypeAsync(meetingCode, taskId, backlogType);
        await _meetingHubContext.Clients
            .GroupExcept(
                meetingCode,
                GetRequestingConnectionId())
            .ChangeTaskBacklogTypeAsync(taskId, backlogType);

    }

    public async Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync(string meetingCode)
    {
        var finalTaskStatesAsync = await _cacheService.GetFinalEvaluationsAsync(meetingCode);
        return finalTaskStatesAsync;
    }

    private Dictionary<Guid, BacklogType> ParseBacklogType(Dictionary<Guid, string> taskBacklogDictionary)
    {
        return taskBacklogDictionary
            .Select(t => new { Key = t.Key, Value = Enum.Parse<BacklogType>(t.Value) })
            .ToDictionary(key => key.Key, value => value.Value);
    }
    private Dictionary<Guid, BacklogType> ParseBacklogType(Dictionary<Guid, int> taskBacklogDictionary)
    {
        return taskBacklogDictionary.ToDictionary(k=>k.Key, v=> (BacklogType)v.Value);
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
