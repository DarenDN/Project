namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

internal sealed class UserRoleModel : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string RoleName { get; }
}
