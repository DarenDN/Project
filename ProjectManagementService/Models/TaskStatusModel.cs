namespace ProjectManagementService.Models;

using Data;

public sealed class TaskStatusModel : DbEntity
{
    public string Name { get; set; }
}
