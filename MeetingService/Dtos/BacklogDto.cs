namespace MeetingService.Dtos;

using MeetingService.Enums;

public record BacklogDto(Guid Id, string Name, BacklogType BacklogType, bool Voted);
