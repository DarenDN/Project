namespace ProjectManagementService.Models;
using Data;
using System.ComponentModel.DataAnnotations;

public sealed class ProjectsIdentity : DbEntity
{
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public Guid IdentityId { get; set; }
}
