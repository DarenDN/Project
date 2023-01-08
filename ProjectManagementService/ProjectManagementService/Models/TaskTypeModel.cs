namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

internal sealed class TaskTypeModel : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string Name { get; set; }
}
