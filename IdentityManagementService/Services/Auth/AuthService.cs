namespace IdentityManagementService.Services.Auth;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using Data;
using Dtos;
using Configurations;

public class AuthService : IAuthService
{
    private Handler.Token.TokenHandler TokenHandler { get; set; }

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
        TokenHandler = new Handler.Token.TokenHandler();
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

        var jwt = TokenHandler.GenerateAccessToken(identity, _securityCfg.Token, _securityCfg.AccessTokenLifeTime);
        var refreshToken = TokenHandler.GenerateRefreshToken(_securityCfg.RefreshTokenLifeTime);
        identity.RefreshToken = refreshToken;
        await _appDbContext.RefreshTokens.AddAsync(refreshToken);
        _appDbContext.Identities.Update(identity);

        await TrySaveChangesAsync();

        TokenHandler.SetRefreshToken(refreshToken.Token, refreshToken.Expires, _httpContextAccessor.HttpContext);

        return jwt;
    }

    public async Task LogoutAsync()
    {
        var requestCookieToken = TokenHandler.GetCookieRefreshToken(_httpContextAccessor.HttpContext);
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

        TokenHandler.DeleteRefreshToken(_httpContextAccessor.HttpContext);
    }

    /// <summary>
    /// Updates refresh token and creates new access token
    /// </summary>
    /// <returns>New access token</returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="SecurityTokenExpiredException"></exception>
    public async Task<string> RefreshTokensAsync()
    {
        var refreshToken = TokenHandler.GetCookieRefreshToken(_httpContextAccessor.HttpContext);

        if (refreshToken is null)
        {
            throw new ArgumentNullException(nameof(refreshToken));
        }

        var identity = await _appDbContext.Identities
            .Include(i => i.RefreshToken)
            .Include(i=>i.UserInfo)
            .FirstOrDefaultAsync(i => i.RefreshToken != null
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

        var newAccessToken = TokenHandler.GenerateAccessToken(identity, _securityCfg.Token, _securityCfg.AccessTokenLifeTime);
        var newRefreshToken = TokenHandler.GenerateRefreshToken(_securityCfg.RefreshTokenLifeTime);

        await _appDbContext.RefreshTokens.AddAsync(newRefreshToken);
        identity.RefreshToken = newRefreshToken;
        _appDbContext.Identities.Update(identity);

        TokenHandler.SetRefreshToken(newRefreshToken.Token, newRefreshToken.Expires, _httpContextAccessor.HttpContext);

        await TrySaveChangesAsync();

        return newAccessToken;
    }

    public async Task<bool> IsAuthorized()
    {
        var refreshTokenString = TokenHandler.GetCookieRefreshToken(_httpContextAccessor.HttpContext);

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

    private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.Unicode.GetBytes(password));

        return computedHash.SequenceEqual(passwordHash);
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
