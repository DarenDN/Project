namespace UserRelatedModelsLibrary.Models;

using System.ComponentModel.DataAnnotations;

public sealed class Identity
{
    [Required]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Login { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public UserInfo UserInfo { get; set; }
}
