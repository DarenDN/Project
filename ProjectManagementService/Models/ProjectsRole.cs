namespace ProjectManagementService.Models;

using Data;
using System.ComponentModel.DataAnnotations;

public sealed class ProjectsRole : DbEntity
{
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public Guid RoleId { get; set; }
}
