namespace ProjectManagementService.Dtos.Task;

public sealed record CreateTaskDto(
    string Title, 
    string Description,
    Guid StatusId,
    Guid TypeId, 
    Guid CreatorId,
    Guid DashboardId);
