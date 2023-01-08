namespace ProjectManagementService.Models;

using Data;

internal sealed class TaskStatusModel : DbEntity
{
    public string Name { get; set; }
}
