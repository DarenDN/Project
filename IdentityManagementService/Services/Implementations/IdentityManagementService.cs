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
    private readonly SecurityConfiguration _securityCfg;
    private readonly IdentityManagementDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityManagementService(
        IdentityManagementDbContext appDbContext,
        IOptions<SecurityConfiguration> options,
        IHttpContextAccessor httpContextAccessor)
    {
        _securityCfg = options.Value;
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    private const string RefreshTokenKeyWord = "refreshToken";

    public async Task<bool> RegisterUserAsync(RegisterUserDto userAuthDto)
    {
        return await CreateUserAsync(userAuthDto);
    }

    /// <summary>
    /// Sets refresh token and creates an access token
    /// </summary>
    /// <param name="userAuthDto"></param>
    /// <returns>Access token</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="KeyNotFoundException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public async Task<string> LoginAsync(UserAuthDto userAuthDto)
    {
        var userLogin = userAuthDto.Login;
        var userPass = userAuthDto.Password;
        if (string.IsNullOrWhiteSpace(userLogin))
        {
            throw new ArgumentNullException($"Empty argument value: {nameof(userAuthDto.Login)}");
        }
        if (string.IsNullOrWhiteSpace(userPass))
        {
            throw new ArgumentNullException($"Empty argument value: {nameof(userAuthDto.Password)}");
        }

        var user = await _appDbContext.Identities
            .Include(i=> i.UserInfo)
            .ThenInclude(ui=> ui.Role)
            .Include(i=> i.RefreshToken)
            .FirstOrDefaultAsync(u => u.Login == userLogin);
        if (user is null)
        {
            throw new ArgumentException($"Login {userLogin} does not exist");
        }

        if (!VerifyPassword(userPass, user.PasswordHash, user.PasswordSalt))
        {
            throw new ArgumentException("Invalid password");
        }

        if (user.RefreshToken is not null)
        {
            _appDbContext.RefreshTokens.Remove(user.RefreshToken);
            user.RefreshToken = null;
        }

        var jwt = GenerateAccessToken(user);
        var refreshToken = GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        _appDbContext.Update(user);

        try
        {
            var success = await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO something
        }

        SetRefreshToken(refreshToken.Token, refreshToken.Expires);
        
        return jwt;
    }

    public async Task<bool> LogoutAsync()
    {
        var requestCookieToken = GetCookieRefreshToken();
        if(string.IsNullOrWhiteSpace(requestCookieToken))
        {
            return true;
        }

        var identity = await _appDbContext.Identities
            .Include(i=>i.RefreshToken)
            .FirstOrDefaultAsync(i => i.RefreshToken != null 
        && i.RefreshToken.Token == requestCookieToken);

        if (identity is null)
        {
            return true;
        }

        _appDbContext.RefreshTokens.Remove(identity.RefreshToken);
        identity.RefreshToken = null;
        _appDbContext.Identities.Update(identity);

        try
        {
            var success = await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO
        }

        DeleteRefreshToken();

        return true;
    }

    public async Task<bool> CreateUserAsync(RegisterUserDto registerUserDto)
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

        // TODO manage no role possibility smh
        var role = _appDbContext.UserRoles.FirstOrDefault(r => r.Id == registerUserDto.RoleId);

        if (role is null)
        {
            throw new NullReferenceException(role.GetType().ToString());
        }

        var (passwordHash, passwordSalt) = await CreatePasswordHashAndSaltAsync(newUserPass);

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
            Login = newUserLogin,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            UserInfo = newUserInfo
        };

        _appDbContext.UserInfos.Add(newUserInfo);
        _appDbContext.Identities.Add(newIdentity);

        await _appDbContext.SaveChangesAsync();

        return true;
    }

    public async Task<bool> DeleteUserAsync(Guid? identityId)
    {
        var identityToDelete = await _appDbContext.Identities
            .Include(i=>i.UserInfo)
            .Include(i=>i.RefreshToken)
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

        var success = await _appDbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Updates refresh token and creates new access token
    /// </summary>
    /// <returns>New access token</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="SecurityTokenExpiredException"></exception>
    public async Task<string> RefreshTokensAsync()
    {
        var refreshToken = GetCookieRefreshToken();

        if(refreshToken is null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var identity = await _appDbContext.Identities.Include(i=>i.RefreshToken).FirstOrDefaultAsync(i => i.RefreshToken != null
            && i.RefreshToken.Token == refreshToken);
        if (identity is null)
        {
            throw new ArgumentNullException();
        }

        var oldRefreshToken = identity.RefreshToken;
        _appDbContext.RefreshTokens.Remove(oldRefreshToken);
        if (oldRefreshToken.Expires < DateTime.Now)
        {
            identity.RefreshToken = null;
            _appDbContext.Identities.Update(identity);
            try
            {
                await _appDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // TODO
            }

            throw new SecurityTokenExpiredException();
        }

        var newAccessToken = GenerateAccessToken(identity);
        var newRefreshToken = GenerateRefreshToken();

        await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
        identity.RefreshToken = newRefreshToken;
        _appDbContext.Identities.Update(identity);

        SetRefreshToken(newRefreshToken.Token, newRefreshToken.Expires);

        try
        {
            var success = await _appDbContext.SaveChangesAsync();
        }
        catch 
        {
            // TODO
        }

        return newAccessToken;
    }

    public async Task<bool> CreateUserRoleAsync(string? newRoleName)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var role = await _appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Name == newRoleName);
        if (role != null)
        {
            var existsInProject = await _appDbContext.ProjectsRoles
                .Where(r => r.ProjectId == projectId)
                .AnyAsync(r => r.RoleId == role.Id);

            if (existsInProject)
            {
                throw new ArgumentException(nameof(newRoleName));
            }

            var projectRole = new ProjectsRole
            {
                ProjectId = projectId.Value,
                RoleId = role.Id
            };

            await _appDbContext.ProjectsRoles.AddAsync(projectRole);
        }
        else
        {
            var newRole = new UserRole
            {
                Name = newRoleName
            };

            var newProjectRole = new ProjectsRole
            {
                ProjectId = projectId.Value,
                RoleId = newRole.Id
            };

            await _appDbContext.UserRoles.AddAsync(newRole);
            await _appDbContext.ProjectsRoles.AddAsync(newProjectRole);
        }

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO
        }

        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<bool> DeleteUserRoleAsync(Guid? roleId)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();

        if (projectId is null)
        {
            throw new ArgumentNullException();
        }

        if (roleId is null)
        {
            throw new ArgumentNullException(nameof(roleId));
        }

        var roleInProjectsCount = await _appDbContext.ProjectsRoles.CountAsync(r => r.RoleId == roleId);

        if (roleInProjectsCount > 1)
        {
            var projectRole = await _appDbContext.ProjectsRoles.FirstOrDefaultAsync(r => r.ProjectId == projectId 
            && r.RoleId == roleId);

            _appDbContext.ProjectsRoles.Remove(projectRole);
        }
        else if (roleInProjectsCount == 1)
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(r=>r.Id == roleId);
            var projectRole = await _appDbContext.ProjectsRoles.FirstOrDefaultAsync(r => r.ProjectId == projectId
            && r.RoleId == roleId);

            _appDbContext.UserRoles.Remove(userRole);
            _appDbContext.ProjectsRoles.Remove(projectRole);
        }
        else
        {
            // TODO
        }

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO
        }

        return true;
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<List<UserRoleDto>> GetAllUserRolesAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();

        if (projectId is null)
        {
            throw new ArgumentNullException(nameof(projectId));
        } 

        var roles = await _appDbContext.UserRoles
            .Join(_appDbContext.ProjectsRoles.Where(r => r.ProjectId == projectId),
                userRole => userRole.Id,
                projRole => projRole.RoleId,
                (userRole, projRole) => userRole)
            .Distinct()
            .Select(r => new UserRoleDto(r.Id, r.Name))
            .ToListAsync();

        return roles;
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
        var refreshTokenLifeTime = TimeSpan.Parse(_securityCfg.RefreshTokenLifeTime).Ticks;
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Token = token,
            Expires = DateTime.Now.AddTicks(refreshTokenLifeTime)
        };

        return refreshToken;
    }

    private async Task<bool> LoginExistsAsync(string login)
    {
        return await _appDbContext.Identities.AnyAsync(i => i.Login == login);
    }

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.Unicode.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
    }

    private void SetRefreshToken(string token, DateTime expires)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires
        };
        var context = _httpContextAccessor.HttpContext;

        context.Response.Cookies.Append(RefreshTokenKeyWord, token, cookieOptions);
    }

    private void DeleteRefreshToken()
    {
        var context = _httpContextAccessor.HttpContext;

        context.Response.Cookies.Delete(RefreshTokenKeyWord);
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
            .Include(i=>i.UserInfo)
            .FirstOrDefaultAsync(i => i.Id == Guid.Parse(identityId));

        var projectId = identity?.UserInfo.ProjectId;

        return projectId;
    }

    private string? GetCookieRefreshToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenKeyWord];
        return token;
    }
}
