namespace ProjectManagementService.Dtos.Sprint;

public sealed record SprintDto(Guid Id, string Name, DateTime DateStart, DateTime DateEnd, string? Description);
