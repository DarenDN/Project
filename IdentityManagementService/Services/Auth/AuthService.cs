namespace IdentityManagementService.Services.IdentityManagement;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Data;
using Dtos;
using Models;
using Configurations;

public class AuthService : IAuthService
{
    private readonly SecurityConfiguration _securityCfg;
    private readonly IdentityManagementDbContext _appDbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        IdentityManagementDbContext appDbContext,
        IOptions<SecurityConfiguration> options,
        IHttpContextAccessor httpContextAccessor)
    {
        _securityCfg = options.Value;
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    private const string RefreshTokenKeyWord = "refreshTokenString";

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

        var identity = await _appDbContext.Identities
            .Include(i => i.UserInfo)
            .Include(i => i.RefreshToken)
            .FirstOrDefaultAsync(u => u.Login == userLogin);
        if (identity is null)
        {
            throw new ArgumentException($"Login {userLogin} does not exist");
        }

        if (!VerifyPassword(userPass, identity.PasswordHash, identity.PasswordSalt))
        {
            throw new ArgumentException("Invalid password");
        }
        var token = identity.RefreshToken;
        if (token is not null)
        {
            _appDbContext.RefreshTokens.Remove(token);
            identity.RefreshToken = null;
        }

        var jwt = GenerateAccessToken(identity);
        var refreshToken = GenerateRefreshToken();
        identity.RefreshToken = refreshToken;
        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        _appDbContext.Identities.Update(identity);

        await TrySaveChangesAsync();

        SetRefreshToken(refreshToken.Token, refreshToken.Expires);

        return jwt;
    }

    public async Task LogoutAsync()
    {
        var requestCookieToken = GetCookieRefreshToken();
        if (string.IsNullOrWhiteSpace(requestCookieToken))
        {
            throw new ArgumentNullException(nameof(requestCookieToken));
        }

        var identity = await _appDbContext.Identities
            .Include(i => i.RefreshToken)
            .FirstOrDefaultAsync(i => i.RefreshToken != null
        && i.RefreshToken.Token == requestCookieToken);

        if (identity is null)
        {
            throw new ArgumentNullException();
        }

        _appDbContext.RefreshTokens.Remove(identity.RefreshToken);
        identity.RefreshToken = null;
        _appDbContext.Identities.Update(identity);

        await TrySaveChangesAsync();

        DeleteRefreshToken();
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

        if (refreshToken is null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var identity = await _appDbContext.Identities.Include(i => i.RefreshToken).FirstOrDefaultAsync(i => i.RefreshToken != null
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
            await TrySaveChangesAsync();

            throw new SecurityTokenExpiredException();
        }

        var newAccessToken = GenerateAccessToken(identity);
        var newRefreshToken = GenerateRefreshToken();

        await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
        identity.RefreshToken = newRefreshToken;
        _appDbContext.Identities.Update(identity);

        SetRefreshToken(newRefreshToken.Token, newRefreshToken.Expires);

        await TrySaveChangesAsync();

        return newAccessToken;
    }

    public async Task<bool> IsAuthorized()
    {
        var refreshTokenString = GetCookieRefreshToken();

        if (refreshTokenString is null)
        {
            return false;
        }

        var identity = await _appDbContext.Identities.Include(i => i.RefreshToken).FirstOrDefaultAsync(i => i.RefreshToken != null
            && i.RefreshToken.Token == refreshTokenString);
        if (identity is null)
        {
            throw new ArgumentNullException();
        }

        var refreshToken = identity.RefreshToken;
        if (refreshToken.Expires < DateTime.Now)
        {
            return false;
        }

        return true;
    }

    private string GenerateAccessToken(Identity user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login),
            new Claim(ClaimTypes.Role, user.UserInfo.RoleId.ToString()),
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

    private string? GetCookieRefreshToken()
    {
        var token = _httpContextAccessor.HttpContext?.Request.Cookies[RefreshTokenKeyWord];
        return token;
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
