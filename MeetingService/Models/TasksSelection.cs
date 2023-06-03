namespace MeetingService.Models;

public class TasksSelection
{
    public string Code { get; set; }
    public Guid? ActiveTask { get; set; } = null!;
    public Dictionary<Guid, bool> TasksOpened { get; set; } = new Dictionary<Guid, bool>();
    public HashSet<Guid?> SprintBacklog { get; set; } = new HashSet<Guid?>();
    public HashSet<Guid?> ProjectBacklog { get; set; } = new HashSet<Guid?>();
}
