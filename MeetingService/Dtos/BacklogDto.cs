namespace MeetingService.Dtos;

using MeetingService.Enums;

public record BacklogDto(BacklogType BacklogType, bool Voted);
