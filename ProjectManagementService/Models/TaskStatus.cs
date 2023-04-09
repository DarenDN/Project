namespace ProjectManagementService.Models;

using Data;
using System.ComponentModel.DataAnnotations;

public sealed class TaskStatus : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
}
