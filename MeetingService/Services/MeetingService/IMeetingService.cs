namespace MeetingService.Services.MeetingService;

using Dtos;
using Enums;

public interface IMeetingService
{
    Task<IEnumerable<TaskSprintEvaluationInfo>> GetFinalEvaluationsAsync(string meetingCode);
    Task<CurrentTaskStateDto> ChangeActiveTaskAndNotifyAsync(string meetingCode, Guid taskId);
    Task ShowEvaluationsAsync(string meetingCode, Guid taskId);
    Task ReevaluateAsync(string meetingCode, Guid taskId);
    Task<string> GetMeetingAsync(Guid projectId);
    Task<string> CreateMeetingAndJoinAsync(Guid projectId, Dictionary<Guid, int> tasks);
    Task<string> CreateMeetingAndJoinAsync(Guid projectId, Dictionary<Guid, BacklogType> tasks);
    Task UpdateUserEvaluationAndNotifyAsync(string meetingCode, TaskEvaluationDto evaluationDto);
    Task EvaluateTaskFinalAndNotifyAsync(string meetingCode, TaskEvaluationDto evaluationDto);
    Task ChangeTaskBacklogTypeAndNotifyAsync(string meetingCode, Guid taskId, BacklogType backlogType);

    Task<MeetingStateDto> JoinMeetingAndNotifyAsync(string meetingCode);
    Task<MeetingStateDto> GetCurrentMeetingStateAsync(string meetingCode);
    Task LeaveMeetingAndNotifyAsync(string meetingCode);
}
