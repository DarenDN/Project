namespace ProjectManagementService.Dtos.Sprint;

public record CreateSprintDto(string? Name, string Description, DateTime DateStart, DateTime DateEnd);
