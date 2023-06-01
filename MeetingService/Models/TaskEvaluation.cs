namespace MeetingService.Models;

public class TaskEvaluation
{
    public Guid Id { get; set; }

    public int? EvaluationPoints { get; set; } = null!;

    public TimeSpan? EvaluationTime { get; set; } = null!;
}
