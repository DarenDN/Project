namespace MeetingService.Hubs;

using Models;

public interface IMeetingHub
{
    Task MeetingUpdated(Meeting meeting);
}
