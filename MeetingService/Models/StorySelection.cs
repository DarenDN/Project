namespace MeetingService.Models;

public class StorySelection
{
    Guid Id { get; set; }

    Guid? ActiveStory { get; set; } = null!;

    List<Guid?> SprintBacklog { get; set; } = new List<Guid?>();

    List<Guid?> ProjectBacklog { get; set; } = new List<Guid?>();
}
