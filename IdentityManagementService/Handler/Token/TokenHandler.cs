namespace IdentityManagementService.Handler.Token;

using Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class TokenHandler
{
    private const string RefreshTokenKeyWord = "refreshTokenString";

    public TokenHandler()
    {
    }

    public string GenerateAccessToken(Identity user, string tokenString, string accessTokenLifeTimeString)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

        if (user.UserInfo?.RoleId is not null)
        {
            claims.Add(new Claim(ClaimTypes.Role, user.UserInfo.RoleId.ToString()));
        }

        var key = new SymmetricSecurityKey(Encoding.Unicode.GetBytes(tokenString));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var accessTokenLifeTime = TimeSpan.Parse(accessTokenLifeTimeString).Ticks;
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddTicks(accessTokenLifeTime),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    public RefreshToken GenerateRefreshToken(string refreshTokenLifeTimeString)
    {
        var refreshTokenLifeTime = TimeSpan.Parse(refreshTokenLifeTimeString).Ticks;
        var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        var refreshToken = new RefreshToken
        {
            Token = token,
            Expires = DateTime.Now.AddTicks(refreshTokenLifeTime)
        };

        return refreshToken;
    }

    public void SetRefreshToken(string token, DateTime expires, HttpContext httpContext)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = expires
        };

        httpContext.Response.Cookies.Append(RefreshTokenKeyWord, token, cookieOptions);
    }

    public void DeleteRefreshToken(HttpContext httpContext)
    {
        httpContext.Response.Cookies.Delete(RefreshTokenKeyWord);
    }

    public string? GetCookieRefreshToken(HttpContext httpContext)
    {
        var token = httpContext.Request.Cookies[RefreshTokenKeyWord];
        return token;
    }
}
