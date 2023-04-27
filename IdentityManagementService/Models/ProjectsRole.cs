namespace IdentityManagementService.Models;

using IdentityManagementService.Data;

public sealed class ProjectsRole : DbEntity
{
    public Guid PrijectId { get; set; }
    public Guid RoleId { get; set; }
}
