namespace ProjectManagementService.Models;

using Data;
using System.ComponentModel.DataAnnotations;

public sealed class TaskState : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Name { get; set; }

    // TODO prev status
    // TODO next status
}
