namespace MeetingService.Hubs;

using Dtos;

public interface IMeetingHub
{
    // TODO Task TaskStateChangedAsync(CurrentTaskStateDto currentTaskDto)
    Task ChangeActiveTaskAsync(CurrentTaskStateDto currentTaskDto);
    Task EvaluateTaskAsync(CurrentTaskStateDto evaluationDto);
    Task EvaluateTaskFinalAsync(CurrentTaskStateDto finalEvaluation);
    Task ReevaluateAsync(CurrentTaskStateDto taskId);
    Task UpdateEvaluationAsync(Guid participantId, EvaluationDto evaluationDto);
    //
    Task ShowEvaluationsAsync(Guid taskId);

    Task RemoveFromSprintBacklogAsync(Guid taskId);
    Task AddToSprintBacklogAsync(Guid taskId);

    Task JoinMeetingAsync(Guid participantId);
    Task LeaveMeetingAsync(Guid participantId);

    Task DeleteMeetingAsync();
}
