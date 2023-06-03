namespace MeetingService.Dtos;

public record TaskSprintEvaluationInfo(Guid TaskId, bool InSprint, int? EvaluationPoints, TimeSpan? EvaluationTime);
