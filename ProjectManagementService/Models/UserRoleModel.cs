namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

// TODO change to Enum
public sealed class UserRoleModel : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string RoleName { get; }
}
