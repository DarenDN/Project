namespace MeetingService.Models;

public class EvaluatingObject
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    // TODO evaluation type?
    public  Evauation { get; set; }
}
