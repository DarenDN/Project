namespace MeetingService.Models;

public class EvaluatingObject
{
    public Guid Id { get; set; }

    public int? EvaluationPoints { get; set; } = null!;

    public DateTime? EvaluationTime { get; set; } = null!;
}
