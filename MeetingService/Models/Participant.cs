namespace MeetingService.Models;

public class Participant
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ConnectionId { get; set; }

    public override int GetHashCode()
    {
        return $"{Id.ToString()}{Name}{ConnectionId}".GetHashCode();
    }

    public override bool Equals(object? obj)
    {
        var objParsed = obj as Participant;
        return objParsed != null && objParsed.Id == Id;
    }
}
