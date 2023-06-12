namespace ProjectManagementService.Dtos.Task;
public sealed record TaskDataDto(
    Guid Id, 
    string Title, 
    string Description,
    string Status, 
    string Type, 
    TimeSpan? EstimationTime,
    int? EstimationPoint,
    Guid? PerformerId, 
    Guid CreatorId);

// TODO time spent on the task. prob List of dtos
// TODO timing 
