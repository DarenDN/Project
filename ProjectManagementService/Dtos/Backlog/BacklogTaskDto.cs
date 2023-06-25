namespace ProjectManagementService.Dtos.Backlog
{
    public record BacklogTaskDto(Guid TaskId, string Name, TimeSpan? EstimationTime, int? EstimationPoint, int BacklogType);
}
