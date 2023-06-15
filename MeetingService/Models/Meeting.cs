namespace MeetingService.Models;

public class Meeting
{
    public string MeetingCode { get; set; }
    public List<Participant> Participants { get; set; } = new List<Participant>();
    public string EvaluationsCode { get; set; }
    public string TaskSelectionCode { get; set; }
}
