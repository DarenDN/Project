namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

internal sealed class UserModel : DbEntity
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string SecondName { get; set; }

    [Required]
    [MaxLength(50)]
    public string MiddleName { get; set; }

    [Required]
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    public UserRoleModel Role { get; set; }
}
