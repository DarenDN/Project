namespace MeetingService.Services.MeetingService;

using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Http;
using Models;
using Hubs;
using CacheService;

public sealed class MeetingService : IMeetingService
{
    private readonly ICacheService _cacheService;
    private readonly IHttpContextAccessor _httpContextAccesor;
    private readonly IHubContext<MeetingHub, IMeetingHub> _meetingHubContext;

    public MeetingService(ICacheService cacheService, 
        IHubContext<MeetingHub, IMeetingHub> meetingHubContext,
        IHttpContextAccessor httpContextAccesor)
    {
        _cacheService = cacheService;
        _meetingHubContext = meetingHubContext;
        _httpContextAccesor = httpContextAccesor;
    }

    public async Task ChangeActiveStoryAsync(Guid storyId)
    {
        throw new NotImplementedException();
    }

    public async Task ShowEvaluationsAsync()
    {
        throw new NotImplementedException();
    }

    public async Task ReevaluateAsync(Guid storyId)
    {
        throw new NotImplementedException();
    }

    public async Task EvaluateStoryAsync(Dtos.EvaluationDto evaluationDto)
    {
        throw new NotImplementedException();
    }

    public async Task RemoveFromSprintBacklogAsync(Guid storyId)
    {
        throw new NotImplementedException();
    }

    public async Task AddToSprintBacklogAsync(Guid storyId)
    {
        throw new NotImplementedException();
    }

    public async Task JoinMeetingAsync(Guid participantId)
    {
        throw new NotImplementedException();
    }

    public async Task LeaveMeetingAsync(Guid participantId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateEvaluationAsync(Guid participantId, Dtos.EvaluationDto evaluationDto)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteMeetingAsync(Guid projectId)
    {
        throw new NotImplementedException();
    }
}
