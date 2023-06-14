namespace MeetingService.Dtos;

using MeetingService.Enums;

public record BacklogTaskDto(Guid TaskId, TimeSpan? EstimationTime, int? EstimationPoint, BacklogType BacklogType);
