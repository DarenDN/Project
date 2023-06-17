
namespace ProjectManagementService.Dtos.Task;
using State;

public sealed record TaskDataDto(
    Guid Id, 
    string Title, 
    string Description,
    string Status, 
    string Type, 
    TimeSpan? EstimationTime,
    int? EstimationPoint,
    IEnumerable<StateDto> NextStates,
    Guid? PerformerId, 
    Guid CreatorId);

// TODO time spent on the task. prob List of dtos
// TODO timing 
