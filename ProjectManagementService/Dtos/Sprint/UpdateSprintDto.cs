namespace ProjectManagementService.Dtos.Sprint;

public record UpdateSprintDto(Guid? SprintId, string? Title, string? Description);
