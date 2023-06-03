namespace MeetingService.Dtos;

public record TaskEvaluationDto(Guid TaskId, int? EvaluationPoints, TimeSpan? EvaluationTime);
