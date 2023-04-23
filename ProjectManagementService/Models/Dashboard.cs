namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class Dashboard : DbEntity
{
    [Required]
    [MaxLength(40)]
    public string Title { get; set; }

    public string Description { get; set; }

    public List<Guid> AllowedUserRoles { get; set; }

    public Project Project { get; set; }

    public List<Task> Tasks { get; set; } = new List<Task>();
}
