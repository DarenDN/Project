namespace MeetingService.Hubs;

using Dtos;
using Models;

public interface IMeetingHub
{
    Task ChangeActiveStoryAsync(Guid storyId);
    Task ShowEvaluationsAsync();
    Task ReevaluateAsync(Guid storyId);

    Task EvaluateStoryAsync(EvaluationDto evaluationDto);

    Task RemoveFromSprintBacklogAsync(Guid storyId);
    Task AddToSprintBacklogAsync(Guid storyId);

    Task JoinMeetingAsync(Guid participantId);
    Task LeaveMeetingAsync(Guid participantId);

    Task UpdateEvaluationAsync(Guid participantId, EvaluationDto evaluationDto);

    Task DeleteMeetingAsync();
}
