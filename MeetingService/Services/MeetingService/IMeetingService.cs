namespace MeetingService.Services.MeetingService;

using Dtos;

public interface IMeetingService
{
    // Remove all projectId
    Task ChangeActiveStoryAsync(Guid storyId);
    Task ShowEvaluationsAsync();
    Task ReevaluateAsync(Guid storyId);

    Task EvaluateStoryAsync(EvaluationDto evaluationDto);

    Task RemoveFromSprintBacklogAsync(Guid storyId);
    Task AddToSprintBacklogAsync(Guid storyId);

    Task JoinMeetingAsync(Guid participantId);
    Task LeaveMeetingAsync(Guid participantId);

    Task UpdateEvaluationAsync(Guid participantId, EvaluationDto evaluationDto);

    Task DeleteMeetingAsync(Guid projectId);
}
