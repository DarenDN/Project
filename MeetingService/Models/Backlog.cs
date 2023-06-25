namespace MeetingService.Models;
using MeetingService.Enums;

public class Backlog
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public BacklogType BacklogType { get; set; }
}
