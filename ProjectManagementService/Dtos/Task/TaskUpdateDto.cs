namespace ProjectManagementService.Dtos.Task;

public record TaskUpdateDto
    (
    Guid Id,
    string Title,
    string Description,
    Guid Status,
    Guid Type,
    Guid? PerformerId,
    Guid? SprintId
    );
