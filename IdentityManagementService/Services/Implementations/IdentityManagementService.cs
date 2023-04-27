namespace IdentityManagementService.Services.Implementations;

using Interfaces;
using Data;
using Dtos;
using Models;
using Configurations;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;

public class IdentityManagementService : IIdentityManagementService
{
    private SecurityConfiguration _securityCfg;
    private IdentityManagementDbContext _appDbContext;

    public IdentityManagementService(IdentityManagementDbContext appDbContext, IOptions<SecurityConfiguration> options)
    {
        _securityCfg = options.Value;
        _appDbContext = appDbContext;
    }

    public async Task<bool> RegisterUserAsync(RegisterUserDto userAuthDto)
    {
        var newIdentity = await CreateUserAsync(userAuthDto);
        return true;
    }

    public async Task<TokenUserDto> LoginAsync(UserAuthDto userAuthDto)
    {
        var user = _appDbContext.Identities.FirstOrDefault(u => u.Login == userAuthDto.Login);
        if (user is null)
        {
            throw new KeyNotFoundException();
        }

        if (!VerifyPassword(userAuthDto.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new ArgumentException();
        }

        if (user.RefreshToken is not null)
        {
            _appDbContext.RefreshTokens.Remove(user.RefreshToken);
            user.RefreshToken = null;
        }

        var jwt = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();

        await _appDbContext.RefreshTokens.AddAsync(refreshToken);

        user.RefreshToken = refreshToken;
        _appDbContext.Update(user);

        // TODO if errors try catch
        var success = await _appDbContext.SaveChangesAsync();
        var refreshTokenDto = new RefreshTokenDto(refreshToken.Token, refreshToken.Expires);

        return new TokenUserDto(jwt, refreshTokenDto);
    }

    public async Task<bool> LogoutAsync(UserAuthDto userAuthDto)
    {
        var identity = _appDbContext.Identities.FirstOrDefault(u => u.Login == userAuthDto.Login);
        var token = identity.RefreshToken;

        if (token is null)
        {
            return true;
        }

        _appDbContext.RefreshTokens.Remove(token);
        identity.RefreshToken = null;
        _appDbContext.Identities.Update(identity);
        // TODO an event of save failed
        var success = await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<UserAuthDto> CreateUserAsync(RegisterUserDto registerUserDto)
    {
        if (string.IsNullOrWhiteSpace(registerUserDto.Login))
        {
            throw new ArgumentNullException(nameof(registerUserDto.Login));
        }
        if (string.IsNullOrWhiteSpace(registerUserDto.Password))
        {
            throw new ArgumentNullException(nameof(registerUserDto.Password));
        }
        if (await LoginExists(registerUserDto.Login))
        {
            throw new ArgumentException(nameof(registerUserDto.Login));
        }

        var role = _appDbContext.UserRoles.FirstOrDefault(r => r.Id == registerUserDto.RoleId);

        if(role is null)
        {
            throw new NullReferenceException(role.GetType().ToString());
        }

        var (passwordHash, passwordSalt) = await CreatePasswordHashAndSaltAsync(registerUserDto.Password);

        var newUserInfo = new UserInfo
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            MiddleName = registerUserDto.MiddleName,
            ProjectId = registerUserDto.ProjectId,
            Email = registerUserDto.Email,
            Role = role
        };

        var newIdentity = new Identity
        {
            Login = registerUserDto.Login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserInfo = newUserInfo
        };

        _appDbContext.UserInfos.Add(newUserInfo);
        _appDbContext.Identities.Add(newIdentity);

        await _appDbContext.SaveChangesAsync();

        var createdUser = new UserAuthDto(registerUserDto.Login, registerUserDto.Password);

        return createdUser;
    }

    public async Task<bool> DeleteUserAsync(Guid identityId)
    {
        var identityToDelete = await _appDbContext.Identities.FirstOrDefaultAsync(i=>i.Id == identityId);
        if (identityToDelete is null)
        {
            throw new KeyNotFoundException(nameof(identityId));
        }

        var userInfoToDelete = identityToDelete.UserInfo;
        var refreshTokenToDelete = identityToDelete.RefreshToken;

        if (userInfoToDelete is null)
        {
            throw new NullReferenceException(nameof(identityToDelete.UserInfo));
        }

        _appDbContext.UserInfos.Remove(userInfoToDelete);

        if (refreshTokenToDelete is not null)
        {
            _appDbContext.RefreshTokens.Remove(refreshTokenToDelete);
        }

        _appDbContext.Identities.Remove(identityToDelete);

        var success = await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<TokenUserDto> RefreshTokensAsync(string refreshToken)
    {
        var identity = await _appDbContext.Identities.FirstOrDefaultAsync(i => i.RefreshToken != null 
            && i.RefreshToken.Token == refreshToken);
        if (identity is null)
        {
            // TODO 
            throw 
        }
        var oldRefreshToken = identity.RefreshToken;
        _appDbContext.RefreshTokens.Remove(oldRefreshToken);
        if (oldRefreshToken.Expires < DateTime.Now)
        {
            identity.RefreshToken = null;
            _appDbContext.Identities.Update(identity);
            await _appDbContext.SaveChangesAsync();

            throw
        }

        var newAccessToken = GenerateAccessToken(identity);
        var newRefreshToken = GenerateRefreshToken();

        await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
        identity.RefreshToken = newRefreshToken;
        _appDbContext.Identities.Update(identity);

        var success = await _appDbContext.SaveChangesAsync();

        return new TokenUserDto(newAccessToken, new RefreshTokenDto(newRefreshToken.Token, newRefreshToken.Expires));
    }

    public async Task<List<UserRoleDto>> GetAllUserRolesAsync(string identityId)
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="password"></param>
    /// <returns>1st value is PasswordHash, 2nd is SaltHash</returns>
    private async Task<(byte[], byte[])> CreatePasswordHashAndSaltAsync(string password)
    {
        using var hmac = new HMACSHA512();
        using var passwordStream = new MemoryStream(Encoding.Unicode.GetBytes(password));

        var computedPasswordHash = await hmac.ComputeHashAsync(passwordStream);

        return (computedPasswordHash, hmac.Key);
    }

    private string GenerateAccessToken(Identity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.UserInfo.Role.Id.ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(_securityCfg.Token));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var accessTokenLifeTime = TimeSpan.Parse(_securityCfg.AccessTokenLifeTime).Ticks;
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddTicks(accessTokenLifeTime),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private RefreshToken GenerateRefreshToken()
    {
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Token = token,
            Expires = DateTime.Now.AddHours(3)
        };

        return refreshToken;
    }

    private async Task<bool> LoginExists(string login)
    {
        return await _appDbContext.Identities.AnyAsync(i => i.Login == login);
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.Unicode.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }
}
