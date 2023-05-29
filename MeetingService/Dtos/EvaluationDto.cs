namespace MeetingService.Dtos;

public record EvaluationDto(Guid StoryId, int? EvaluationPoints, DateTime? EvaluationTime);
