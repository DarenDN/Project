namespace MeetingService.Models;

public sealed class ParticipantEvaluation
{
    public Guid Id { get; set; }
    public List<Guid> Participants { get; set; }
    public Dictionary<Guid, List<EvaluatingObject>> EvaluationsByParticipant { get; set; }
}
