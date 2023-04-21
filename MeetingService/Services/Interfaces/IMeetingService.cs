namespace MeetingService.Services.Interfaces;

using MeetingService.Models;

public interface IMeetingService
{
    Task<Meeting> GetMeetingAsync(Guid projectId);
    Task<Meeting> JoinMeetingAsync(Participant participant, Guid projectId);
    Task<bool> UpdateMeetingAsync(Meeting meeting, Guid projectId);
    Task<bool> LeaveMeetingAsync(Participant participant, Guid projectId);
    Task<bool> DeleteMeetingAsync(Guid projectId);
}
