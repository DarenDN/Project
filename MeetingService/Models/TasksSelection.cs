namespace MeetingService.Models;

using MeetingService.Enums;

public class TasksSelection
{
    public string Code { get; set; }
    public Guid? ActiveTask { get; set; } = null!;
    public Dictionary<Guid, bool> TasksEvaluationsOpen { get; set; } = new Dictionary<Guid, bool>();
    public List<Backlog> Backlog { get; set; } = new List<Backlog>();
}
