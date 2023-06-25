namespace MeetingService.Hubs;

using Dtos;
using MeetingService.Enums;

public interface IMeetingHub
{
    Task UserJoinedMeetingAsync(ParticipantEvaluationDto participantEvaluationDto);
    Task UserLeavedMeetingAsync(ParticipantIdDto participantDto);
    Task DeleteMeetingAsync();

    // TODO Task TaskStateChangedAsync(CurrentTaskStateDto currentTaskDto)
    Task ChangeActiveTaskAsync(CurrentTaskStateDto currentTaskDto);
    Task ReevaluateAsync(CurrentTaskStateDto currentTaskDto);
    //
    Task UpdateUserEvaluationAsync(CurrentTaskStateDto currentTaskDto);
    Task EvaluateTaskFinalAsync(TaskEvaluationDto evaluationDto);
    Task ShowEvaluationsAsync(TaskDto taskDto);

    Task ChangeTaskBacklogTypeAsync(TaskBacklogChangedDto taskBacklogChangedDto);

    Task TestHub(string testString);
}
