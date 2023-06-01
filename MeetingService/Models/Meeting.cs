namespace MeetingService.Models;

public class Meeting
{
    public string MeetingCode { get; set; }

    /// <summary>
    /// Key: Guid - userId, Value: string - connectionId
    /// </summary>
    public Dictionary<Guid, string> Participants { get; set; }
    public string EvaluationsCode { get; set; }
    public string TaskSelectionCode { get; set; }
}
