namespace ProjectManagementService.Dtos.Task;

public sealed record CreateTaskDto(
    string Title, 
    string? Description,
    Guid TypeId, 
    Guid? SprintId);
