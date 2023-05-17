namespace IdentityManagementService.Services.IdentityManagement;

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
    private readonly IdentityManagementDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityManagementService(
        IdentityManagementDbContext appDbContext,
        IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task RegisterUserAsync(RegisterUserDto userAuthDto)
    {
        await CreateUserAsync(userAuthDto);
    }

    public async Task CreateUserAsync(RegisterUserDto registerUserDto)
    {
        var newUserLogin = registerUserDto.Login;
        var newUserPass = registerUserDto.Password;

        if (string.IsNullOrWhiteSpace(newUserLogin))
        {
            throw new ArgumentNullException(nameof(registerUserDto.Login));
        }
        if (string.IsNullOrWhiteSpace(newUserPass))
        {
            throw new ArgumentNullException(nameof(registerUserDto.Password));
        }
        if (await LoginExistsAsync(newUserLogin))
        {
            throw new ArgumentException(nameof(registerUserDto.Login));
        }

        var (passwordHash, passwordSalt) = await CreatePasswordHashAndSaltAsync(newUserPass);

        var newUserInfo = new UserInfo
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            MiddleName = registerUserDto.MiddleName,
            ProjectId = registerUserDto.ProjectId,
            Email = registerUserDto.Email,
            RoleId = registerUserDto.RoleId
        };

        var newIdentity = new Identity
        {
            Login = newUserLogin,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserInfo = newUserInfo
        };

        _appDbContext.UserInfos.Add(newUserInfo);
        _appDbContext.Identities.Add(newIdentity);

        await TrySaveChangesAsync();
    }

    public async Task DeleteUserAsync(Guid? identityId)
    {
        var identityToDelete = await _appDbContext.Identities
            .Include(i => i.UserInfo)
            .Include(i => i.RefreshToken)
            .FirstOrDefaultAsync(i => i.Id == identityId);
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
        await TrySaveChangesAsync();
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

    private async Task<bool> LoginExistsAsync(string login)
    {
        return await _appDbContext.Identities.AnyAsync(i => i.Login == login);
    }

    private string GetClaimedIdentityId()
    {
        var identityId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        return identityId;
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = GetClaimedIdentityId();
        if (string.IsNullOrWhiteSpace(identityId))
        {
            return Guid.Empty;
        }

        var identity = await _appDbContext.Identities
            .Include(i => i.UserInfo)
            .FirstOrDefaultAsync(i => i.Id == Guid.Parse(identityId));

        var projectId = identity?.UserInfo.ProjectId;

        return projectId;
    }

    public async Task<IEnumerable<UserDto>> GetUsers()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if(projectId is null)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        var users = _appDbContext.Identities
                                    .Include(i => i.UserInfo)
                                    .Where(i => i.UserInfo.ProjectId == projectId)
                                    .Select(i=> new UserDto(i.Id, i.UserInfo.RoleId, i.UserInfo.FirstName, i.UserInfo.LastName, i.UserInfo.MiddleName));

        return users;
    }

    public async Task<UserIdentityDto> GetUser(Guid? identityId)
    {
        var identity = await _appDbContext.Identities.Include(i=>i.UserInfo).FirstOrDefaultAsync(i => i.Id == identityId);
        var userInfo = identity.UserInfo;

        return new UserIdentityDto(
            identity.Id,
            userInfo.RoleId,
            identity.Login,
            userInfo.FirstName,
            userInfo.LastName,
            userInfo.MiddleName,
            userInfo.Email,
            userInfo.RegisterTime);
    }

    private async Task TrySaveChangesAsync()
    {
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
