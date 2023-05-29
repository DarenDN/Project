﻿namespace IdentityManagementService.Services.IdentityManagement;

using Data;
using Dtos;
using Models;
using Handler.Token;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Configurations;
using System.Security.Principal;

public class IdentityManagementService : IIdentityManagementService
{
    private TokenHandler TokenHandler { get; set; }

    private readonly IdentityManagementDbContext _appDbContext;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly SecurityConfiguration _securityCfg;

    public IdentityManagementService(
        IdentityManagementDbContext appDbContext,
        IHttpContextAccessor httpContextAccessor,
        IOptions<SecurityConfiguration> securityCfg)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
        _securityCfg = securityCfg.Value;
        TokenHandler = new TokenHandler();
    }

    /// <summary>
    /// registers user, sets up refresh token and access token (jwt)
    /// </summary>
    /// <param name="registerUserDto"></param>
    /// <returns>access token (jwt)</returns>
    public async Task<string> RegisterUserIdentityAsync(RegisterIdentityDto registerUserDto)
    {
        var createdIdentity = await RegisterNewIdentityAsync(registerUserDto);
        var refreshToken = TokenHandler.GenerateRefreshToken(_securityCfg.RefreshTokenLifeTime);
        var accessToken = TokenHandler.GenerateAccessToken(createdIdentity, _securityCfg.Token,_securityCfg.AccessTokenLifeTime);
        TokenHandler.SetRefreshToken(refreshToken.Token, refreshToken.Expires, _httpContextAccessor.HttpContext);
        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        createdIdentity.RefreshToken = refreshToken;
        await TrySaveChangesAsync();
        return accessToken;
    }

    public async Task RegisterUserDataAsync(RegisterUserDataDto registerUserDto)
    {
        await RegisterNewDataAsync(registerUserDto);
    }

    public async Task<Guid> CreateUserAsync(CreateUserDto createUserDto)
    {
        var identityId = await CreateNewUser(createUserDto);
        return identityId;
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

    public async Task<IEnumerable<UserDto>> GetUsersAsync()
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

    public async Task<UserIdentityDto> GetUserAsync(Guid? identityId)
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

    public async Task<CurrentUserInfoDto> GetUserAsync()
    {
        var identityId = GetClaimedIdentityId();
        if(string.IsNullOrWhiteSpace(identityId))
        {
            throw new ArgumentException(nameof(identityId));
        }

        var identity = await _appDbContext.Identities.Include(i=>i.UserInfo).FirstOrDefaultAsync(i => i.Id == Guid.Parse(identityId));
        var userInfo = identity.UserInfo;

        return new CurrentUserInfoDto(userInfo.FirstName, userInfo.LastName, userInfo.MiddleName);
    }

    private async Task<Identity> RegisterNewIdentityAsync(RegisterIdentityDto registerUserDto)
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


        var newIdentity = new Identity
        {
            Login = newUserLogin,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
        };

        _appDbContext.Identities.AddAsync(newIdentity);

        return newIdentity;
    }
    private async Task RegisterNewDataAsync(RegisterUserDataDto registerUserDto)
    {
        var identityIdString = GetClaimedIdentityId();
        if(string.IsNullOrWhiteSpace(identityIdString) || !Guid.TryParse(identityIdString, out var identityId))
        {
            throw new ArgumentException();
        }

        var identity = await _appDbContext.Identities.FirstOrDefaultAsync(i=>i.Id == identityId);

        var newUserInfo = new UserInfo
        {
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName,
            MiddleName = registerUserDto.MiddleName,
            ProjectId = registerUserDto.ProjectId,
            Email = registerUserDto.Email,
            RoleId = registerUserDto.RoleId
        };

        identity.UserInfo = newUserInfo;

        _appDbContext.UserInfos.Add(newUserInfo);
        _appDbContext.Identities.Update(identity);

        await TrySaveChangesAsync();
    }

    private async Task<Guid> CreateNewUser(CreateUserDto createUserDto)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if (projectId is null)
        {
            throw new ArgumentException(nameof(projectId));
        }
        var newUserLogin = createUserDto.Login;
        var newUserPass = createUserDto.Password;

        if (string.IsNullOrWhiteSpace(newUserLogin))
        {
            throw new ArgumentNullException(nameof(createUserDto.Login));
        }
        if (string.IsNullOrWhiteSpace(newUserPass))
        {
            throw new ArgumentNullException(nameof(createUserDto.Password));
        }
        if (await LoginExistsAsync(newUserLogin))
        {
            throw new ArgumentException(nameof(createUserDto.Login));
        }

        var (passwordHash, passwordSalt) = await CreatePasswordHashAndSaltAsync(newUserPass);

        var newUserInfo = new UserInfo
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            MiddleName = createUserDto.MiddleName,
            ProjectId = projectId.Value,
            Email = createUserDto.Email,
            RoleId = createUserDto.RoleId
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

        return newIdentity.Id;
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

    public Task UpdateUserAsync()
    {
        throw new NotImplementedException();
    }

    public async Task SetProjectIdToUserAsync(Guid projectId)
    {
        var identityIdString = GetClaimedIdentityId();
        if(string.IsNullOrWhiteSpace(identityIdString) || !Guid.TryParse(identityIdString, out var identityId))
        {
            throw new ArgumentException(nameof(identityIdString));
        }

        var idnetity = await _appDbContext.Identities.Include(i=>i.UserInfo).FirstOrDefaultAsync(i=>i.Id == identityId);
        var userInfo = idnetity.UserInfo;
        userInfo.ProjectId = projectId;
        _appDbContext.UserInfos.Update(userInfo);
        await TrySaveChangesAsync();
    }
}
