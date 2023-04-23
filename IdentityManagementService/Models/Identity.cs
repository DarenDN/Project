namespace IdentityManagementService.Models;

using Data;

public sealed class Identity : DbEntity
{
    public string Login { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }

    public UserInfo UserInfo { get; set; }
}
