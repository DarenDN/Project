namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class TaskType : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Name { get; set; }
}
