namespace AutentificationService.Models;

using Data;

public sealed class User : DbEntity
{
    public string Login { get; set; }

    public byte[] PasswordHash { get; set; }

    public byte[] PasswordSalt { get; set; }
}
