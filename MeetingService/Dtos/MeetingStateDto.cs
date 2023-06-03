namespace MeetingService.Dtos;

using MeetingService.Enums;
using MeetingService.Models;

public record MeetingStateDto(
    Guid? ActiveTask,
    TaskEvaluation? TaskFinalEvaluations,
    Dictionary<Guid, BacklogType> Backlog,
    IEnumerable<Guid> Participants,
    Dictionary<Guid, TaskEvaluation> EvaluationsByParticipant
    );
