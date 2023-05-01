namespace IdentityManagementService.Models;

using Data;
using System.ComponentModel.DataAnnotations;

public sealed class Identity : DbEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [MaxLength(30)]
    public string Login { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    public RefreshToken? RefreshToken { get; set; }

    [Required]
    public UserInfo UserInfo { get; set; }
}
