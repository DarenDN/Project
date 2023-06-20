namespace MeetingService.Dtos;

public record CreateMeetingDto(string userName, Guid projectId, IEnumerable<BacklogTaskDto> tasks);
