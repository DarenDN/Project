namespace ProjectManagementService.Models;

using System.ComponentModel.DataAnnotations;
using Data;

public sealed class User : DbEntity
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

    public UserRole Role { get; set; }
}
