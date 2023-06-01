namespace MeetingService.Hubs;

using Dtos;
using Models;
using Microsoft.AspNetCore.SignalR;

public class MeetingHub : Hub<IMeetingHub>
{
    public async Task ChangeActiveTaskAsync(Guid taskId, string group)
    {
        await Clients.OthersInGroup(group).ChangeActiveTaskAsync(taskId);
    }

    public async Task ShowEvaluationsAsync(string group)
    {
        await Clients.OthersInGroup(group).ShowEvaluationsAsync();
    }

    public async Task ReevaluateAsync(Guid taskId, string group)
    {
        await Clients.OthersInGroup(group).ReevaluateAsync(taskId);
    }

    public async Task EvaluateTaskAsync(Guid userId, EvaluationDto evaluationDto, string group)
    {
        await Clients.OthersInGroup(group).EvaluateTaskAsync(userId, evaluationDto);
    }

    public async Task EvaluateTaskFinalAsync(FinalEvaluation evaluationDto, string group)
    {
        await Clients.OthersInGroup(group).EvaluateTaskFinalAsync(evaluationDto);
    }

    public async Task RemoveFromSprintBacklogAsync(Guid taskId, string group)
    {
        await Clients.OthersInGroup(group).RemoveFromSprintBacklogAsync(taskId);
    }

    public async Task AddToSprintBacklogAsync(Guid taskId, string group)
    {
        await Clients.OthersInGroup(group).AddToSprintBacklogAsync(taskId);
    }

    public async Task JoinMeetingAsync(Guid participantId, string group)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await Clients.OthersInGroup(group).JoinMeetingAsync(participantId);
    }

    public async Task LeaveMeetingAsync(Guid participantId, string group)
    {
        await Clients.OthersInGroup(group).LeaveMeetingAsync(participantId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, group);
    }

    public async Task UpdateEvaluationAsync(Guid participantId, EvaluationDto evaluationDto, string group)
    {
        await Clients.OthersInGroup(group).UpdateEvaluationAsync(participantId, evaluationDto);
    }

    public async Task DeleteMeetingAsync(string group)
    {
        await Clients.OthersInGroup(group).DeleteMeetingAsync();
    }
}
