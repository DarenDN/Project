namespace IdentityManagementService.Models;

using IdentityManagementService.Data;
using System.ComponentModel.DataAnnotations;

public sealed class UserInfo : DbEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    [MaxLength(50)]
    public string SecondName { get; set; }
    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    public UserRole Role { get; set; }
}
