namespace MeetingService.Dtos;

using MeetingService.Enums;
using MeetingService.Models;

public record MeetingStateDto(
    Guid? ActiveTask,
    TaskEvaluation? TaskFinalEvaluations,
    Dictionary<Guid, BacklogDto> Backlog,
    IEnumerable<ParticipantDto> Participants,
    Dictionary<Guid, TaskEvaluation> EvaluationsByParticipant
    );
