namespace ProjectManagementService.Dtos.Backlog
{
    public record BacklogTaskDto(Guid TaskId, TimeSpan? EstimationTime, int? EstimationPoint, int BacklogType);
}
