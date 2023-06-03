namespace MeetingService.Hubs;

using Dtos;
using MeetingService.Enums;

public interface IMeetingHub
{
    Task UserJoinedMeetingAsync(ParticipantEvaluationDto participantEvaluationDto);
    Task UserLeavedMeetingAsync(Guid participantId);
    Task DeleteMeetingAsync();

    // TODO Task TaskStateChangedAsync(CurrentTaskStateDto currentTaskDto)
    Task ChangeActiveTaskAsync(CurrentTaskStateDto currentTaskDto);
    Task ReevaluateAsync(Guid taskId);
    //
    Task UpdateUserEvaluationAsync(ParticipantEvaluationDto participantEvaluationDto);
    Task EvaluateTaskFinalAsync(TaskEvaluationDto evaluationDto);
    Task ShowEvaluationsAsync(Guid taskId);

    Task ChangeTaskBacklogTypeAsync(Guid taskId, BacklogType backlogType);
}
