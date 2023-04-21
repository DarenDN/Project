namespace MeetingService.Models;

public sealed class Meeting
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<Participant> Participants { get; set; }
    public Dictionary<Guid, List<EvaluatingObject>> EvaluationsByParticipant { get; set; }
}
