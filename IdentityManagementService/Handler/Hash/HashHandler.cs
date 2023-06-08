namespace IdentityManagementService.Handler.Hash;

using System.Security.Cryptography;
using System.Text;

internal class HashHandler
{
    internal async Task<(byte[], byte[])> CreateHashAndSaltAsync(string value)
    {
        using var hmac = new HMACSHA512();
        using var passwordStream = new MemoryStream(Encoding.Unicode.GetBytes(value));

        var computedPasswordHash = await hmac.ComputeHashAsync(passwordStream);

        return (computedPasswordHash, hmac.Key);
    }
}
