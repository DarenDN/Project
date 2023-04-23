namespace IdentityManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

// TODO change to Enum
public sealed class UserRole : DbEntity
{
    [Required]
    [MaxLength(20)]
    public string RoleName { get; }
}
