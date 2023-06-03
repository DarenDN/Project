namespace MeetingService.Dtos;

public record CurrentTaskStateDto(
    Guid Id,
    bool Opened,
    EvaluationDto? FinalEvaluation,
    Dictionary<Guid, EvaluationDto?>? EvaluationByUsers
    );
