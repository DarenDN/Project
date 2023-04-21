namespace MeetingService.Services.Implementations;

using Interfaces;
using Models;
using Hubs;
using Microsoft.AspNetCore.SignalR;

public sealed class MeetingService : IMeetingService
{
    private readonly ICacheService _cacheService;
    private readonly IHubContext<MeetingHub, IMeetingHub> _meetingHubContext;

    public MeetingService(ICacheService cacheService, IHubContext<MeetingHub, IMeetingHub> meetingHubContext)
    {
        _cacheService = cacheService;
        _meetingHubContext = meetingHubContext;
    }

    // TODO need to create meeting at some point

    public async Task<Meeting> GetMeetingAsync(Guid projectId)
    {
        try
        {
            var meeting = await _cacheService.GetAsync(projectId);
            return meeting;
        }
        catch (Exception ex)
        {
            // TODO dif exceptions
            // TODO create new meeting or throw "Meeting is not yet started"?
        }
    }

    public async Task<Meeting> JoinMeetingAsync(Participant participant ,Guid projectId)
    {
        // TODO try catch
        var meeting = await _cacheService.GetAsync(projectId);
        meeting.Participants.Add(participant);
        // TODO try
        await _cacheService.UpdateAsync(meeting, projectId);

        await _meetingHubContext.Clients.Group(projectId.ToString()).MeetingUpdated(meeting);

        return meeting;
    }

    public async Task<bool> UpdateMeetingAsync(Meeting meeting, Guid projectId)
    {
        var isSuccess = await _cacheService.UpdateAsync(meeting, projectId);
        return isSuccess;
    }

    public async Task<bool> LeaveMeetingAsync(Participant participant, Guid projectId)
    {
        // TODO try catch
        var meeting = await _cacheService.GetAsync(projectId);
        // TODO if else
        meeting.Participants.Remove(participant);
        // TODO if !Participants.Any() delete meeting and no MeetingUpdated needed
        // TODO try
        await _cacheService.UpdateAsync(meeting, projectId);

        await _meetingHubContext.Clients.Group(projectId.ToString()).MeetingUpdated(meeting);

        return true;
    }

    public async Task<bool> DeleteMeetingAsync(Guid projectId)
    {
        // TODO some try catch here
        var isSuccess = await _cacheService.DeleteAsync(projectId);
        return isSuccess;
    }
}
