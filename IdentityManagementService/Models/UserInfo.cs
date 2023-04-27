namespace IdentityManagementService.Models;

using Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public sealed class UserInfo : DbEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }

    [MaxLength(50)]
    public string MiddleName { get; set; }

    [Required]
    public string Email { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [Required]
    [Column(TypeName = "timestamp without time zone")]
    public DateTime RegisterTime { get; set; } = DateTime.Now;

    [Required]
    public UserRole Role { get; set; }
}
