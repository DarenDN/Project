namespace ProjectManagementService.Dtos.Sprint;

public sealed record SprintDto(Guid Id, DateTime DateStart, DateTime DateEnd, string? Description);
