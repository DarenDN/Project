namespace MeetingService.Services.MeetingService;

using Dtos;
using Enums;

public interface IMeetingService
{
    Task<IEnumerable<BacklogTaskDto>> GetFinalEvaluationsAsync();
    Task<CurrentTaskStateDto> ChangeActiveTaskAndNotifyAsync( Guid taskId);
    Task ShowEvaluationsAsync( Guid taskId);
    Task ReevaluateAsync(Guid taskId);
    Task DeleteMeetingAsync();
    Task<string> GetMeetingAsync(Guid projectId);
    Task<MeetingStateDto> CreateMeetingAndJoinAsync(string userName, Guid projectId, IEnumerable<BacklogTaskDto> tasks);
    Task UpdateUserEvaluationAndNotifyAsync(TaskEvaluationDto evaluationDto);
    Task EvaluateTaskFinalAndNotifyAsync(TaskEvaluationDto evaluationDto);
    Task ChangeTaskBacklogTypeAndNotifyAsync(Guid taskId, BacklogType backlogType);

    Task<MeetingStateDto> JoinMeetingAndNotifyAsync(string userName);
    Task<MeetingStateDto> GetCurrentMeetingStateAsync();
    Task LeaveMeetingAndNotifyAsync();

    Task TestSignalR();
}
