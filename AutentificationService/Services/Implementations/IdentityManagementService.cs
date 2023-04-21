namespace IdentityManagementService.Services.Implementations;

using Services.Interfaces;
using Data;
using Dtos;
using Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class IdentityManagementService : IIdentityManagementService
{
    private SecurityConfiguration _securityCfg;
    private IdentityManagementDbContext _appDbContext;

    public IdentityManagementService(IdentityManagementDbContext appDbContext, IOptions<SecurityConfiguration> options)
    {
        _securityCfg = options.Value;
        _appDbContext = appDbContext;
    }

    public async Task<UserAuthDto> CreateUserAsync(UserAuthDto userAuthDto)
    {
        if (string.IsNullOrWhiteSpace(userAuthDto.Login) || string.IsNullOrWhiteSpace(userAuthDto.Password))
        {
            // TODO
            return null;
        }

        var (passwordHash, passwordSalt) = await CreatePasswordHashAndSaltAsync(userAuthDto.Password);

        var newUser = new User
        {
            Login = userAuthDto.Login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _appDbContext.Users.Add(newUser);
        await _appDbContext.SaveChangesAsync();

        return userAuthDto;
    }

    public async Task<string> LoginAsync(UserAuthDto userAuthDto)
    {
        var user = _appDbContext.Users.FirstOrDefault(u => u.Login == userAuthDto.Login);
        if (user is null)
        {
            // TODO user not found exc
            throw new NullReferenceException();
        }

        if (!VerifyPassword(userAuthDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            // TODO
            throw new ArgumentException();
        }

        return await GenerateTokenAsync(user);
    }

    public async Task<bool> DeleteUserAsync(Guid userId)
    {
        // TODO actual method
        throw new NotImplementedException();
    }

    private async Task<(byte[], byte[])> CreatePasswordHashAndSaltAsync(string password)
    {
        using var hmac = new HMACSHA512();
        using var passwordStream = new MemoryStream(Encoding.Unicode.GetBytes(password));

        var computedPasswordHash = await hmac.ComputeHashAsync(passwordStream);

        return (computedPasswordHash, hmac.Key);
    }

    private async Task<string> GenerateTokenAsync(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Login)
        };

        // TODO wth is that
        var key = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(_securityCfg.Token));

        // TODO wtf if secutiry algorithm and which is best for me to use?
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.Date.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.Unicode.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
}
