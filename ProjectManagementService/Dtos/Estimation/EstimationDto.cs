namespace ProjectManagementService.Dtos.Estimation;

public class EstimationDto
{
    public Guid TaskId { get; set; }
    public TimeSpan? EstimationInTime { get; set; } = null!;
    public int? EstimationInPoints { get; set; } = null!;
}
