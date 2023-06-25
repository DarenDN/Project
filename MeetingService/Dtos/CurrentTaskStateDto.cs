namespace MeetingService.Dtos;

public record CurrentTaskStateDto(
    Guid Id,
    string Name,
    bool Opened,
    EvaluationDto? FinalEvaluation,
    Dictionary<Guid, EvaluationDto?>? EvaluationByUsers
    );
