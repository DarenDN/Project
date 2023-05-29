namespace IdentityManagementService.Models;

using Data;
using System.ComponentModel.DataAnnotations;

public sealed class Identity : DbEntity
{
    [Required]
    [MaxLength(30)]
    public string Login { get; set; }

    [Required]
    public byte[] PasswordHash { get; set; }

    [Required]
    public byte[] PasswordSalt { get; set; }

    public RefreshToken? RefreshToken { get; set; }

    public UserInfo? UserInfo { get; set; }
}
