namespace MeetingService.Dtos;

using MeetingService.Models;

public record MeetingStateDto(
    Guid? ActiveTask,
    IEnumerable<Guid?> SprintBacklog,
    IEnumerable<Guid?> ProjectBacklog,
    IEnumerable<Guid> Participants,
    Dictionary<Guid, TaskEvaluation?> TaskFinalEvaluations,
    Dictionary<Guid, List<TaskEvaluation>> EvaluationsByParticipant
    );
