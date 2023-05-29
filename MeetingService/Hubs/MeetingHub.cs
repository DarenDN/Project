namespace MeetingService.Hubs;

using MeetingService.Models;
using Microsoft.AspNetCore.SignalR;

public class MeetingHub : Hub<IMeetingHub>
{
    // TODO methods to send states with a help of SignalR
    public async Task MeetingUpdatedAsync(ParticipantEvaluation meeting, string group)
    {
        await Clients.Group(group).MeetingUpdated(meeting);
    }
     
    public async Task JoinMeetingAsync(string group)
    {
        var connectionId = Context.ConnectionId;
        await Groups.AddToGroupAsync(connectionId, group);
    }

    public async Task LeaveMeetingAsync(string group)
    {
        var connectionId = Context.ConnectionId;
        await Groups.RemoveFromGroupAsync(connectionId, group);
    }
}
