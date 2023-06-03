namespace ProjectManagementService.Dtos.Task;

public record TaskSprintEvaluationInfo(Guid TaskId, bool InSprint, int? EvaluationPoints, TimeSpan? EvaluationTime);
