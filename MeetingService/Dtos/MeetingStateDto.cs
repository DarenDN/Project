namespace MeetingService.Dtos;

using MeetingService.Enums;
using MeetingService.Models;

public record MeetingStateDto(
    Guid? ActiveTask,
    TaskEvaluation? TaskFinalEvaluations,
    IEnumerable<BacklogDto> Backlog,
    IEnumerable<ParticipantDto> Participants,
    Dictionary<Guid, TaskEvaluation> EvaluationsByParticipant
    );
