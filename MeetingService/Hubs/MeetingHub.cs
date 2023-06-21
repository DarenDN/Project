namespace MeetingService.Hubs;

using Dtos;
using Models;
using Microsoft.AspNetCore.SignalR;
using MeetingService.Enums;
using Microsoft.AspNetCore.Cors;

[EnableCors("CorsPolicy")]
public class MeetingHub : Hub<IMeetingHub>
{
    public async Task ChangeActiveTaskAsync(string meetingCode, CurrentTaskStateDto currentTaskDto)
    {
        await Clients.OthersInGroup(meetingCode).ChangeActiveTaskAsync(currentTaskDto);
    }

    public async Task ShowEvaluationsAsync(string meetingCode, Guid taskId)
    {
        await Clients.OthersInGroup(meetingCode).ShowEvaluationsAsync(taskId);
    }

    public async Task ReevaluateAsync(string meetingCode, CurrentTaskStateDto currentTaskDto)
    {
        await Clients.OthersInGroup(meetingCode).ReevaluateAsync(currentTaskDto);
    }

    public async Task UpdateUserEvaluationAsync(string meetingCode, ParticipantEvaluationDto participantEvaluationDto)
    {
        await Clients.OthersInGroup(meetingCode).UpdateUserEvaluationAsync(participantEvaluationDto);
    }

    public async Task EvaluateTaskFinalAsync(string meetingCode, TaskEvaluationDto evaluationDto)
    {
        await Clients.OthersInGroup(meetingCode).EvaluateTaskFinalAsync(evaluationDto);
    }

    public async Task ChangeTaskBacklogTypeAsync(string meetingCode, Guid taskId , BacklogType backlogType)
    {
        await Clients.OthersInGroup(meetingCode).ChangeTaskBacklogTypeAsync(taskId, backlogType);
    }

    public async Task JoinMeetingAsync(string meetingCode, ParticipantEvaluationDto participantEvaluationDto)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, meetingCode);
        await Clients.OthersInGroup(meetingCode).UserJoinedMeetingAsync(participantEvaluationDto);
    }

    public async Task LeaveMeetingAsync(string meetingCode, Guid participantId)
    {
        await Clients.OthersInGroup(meetingCode).UserLeavedMeetingAsync(participantId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, meetingCode);
    }

    public async Task UpdateEvaluationAsync(string meetingCode, ParticipantEvaluationDto participantEvaluationDto)
    {
        await Clients.OthersInGroup(meetingCode).UpdateUserEvaluationAsync(participantEvaluationDto);
    }

    public async Task DeleteMeetingAsync(string meetingCode)
    {
        await Clients.OthersInGroup(meetingCode).DeleteMeetingAsync();
    }

    public async Task TestHub(string testString)
    {
        await Clients.Caller.TestHub(testString);
    }
}
