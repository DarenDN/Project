namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Dtos;
using Services.Interfaces;   

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private const string RefreshTokenKeyWord = "refreshToken";
    private IIdentityManagementService _identityManagmentService;

    public AuthenticationController(IIdentityManagementService identManagmentService)
    {
        _identityManagmentService = identManagmentService;
    }

    // TODO create role

    [HttpPost]
    [Route(nameof(RegisterAsync))]
    public async Task<ActionResult> RegisterAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            var isSuccess = await _identityManagmentService.RegisterUserAsync(registerUserDto);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO ex
            return BadRequest();
        }
    }

    [HttpGet]
    [Route(nameof(LoginAsync))]
    public async Task<ActionResult> LoginAsync(UserAuthDto userAuthDto)
    {
        try
        {
            var createdToken = await _identityManagmentService.LoginAsync(userAuthDto);
            SetRefreshToken(createdToken.RefreshToken);
            return Ok(createdToken.JwtToken);
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(LogoutAsync))]
    public async Task<ActionResult> LogoutAsync(UserAuthDto userAuthDto)
    {
        try
        {
            await _identityManagmentService.LogoutAsync(userAuthDto);
            DeleteRefreshToken();
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    [HttpPost, Authorize]
    [Route(nameof(CreateUserAsync))]
    public async Task<ActionResult> CreateUserAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            var createdUser = await _identityManagmentService.CreateUserAsync(registerUserDto);
            return Ok(createdUser);
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete, Authorize]
    [Route(nameof(DeleteUserAsync))]
    public async Task<ActionResult> DeleteUserAsync(Guid identityId)
    {
        try
        {
            await _identityManagmentService.DeleteUserAsync(identityId);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(UpdateRefreshTokenAsync))]
    public async Task<ActionResult> UpdateRefreshTokenAsync()
    {
        try
        {
            var currentRefreshToken = Request.Cookies[RefreshTokenKeyWord];
            var refreshedTokens = await _identityManagmentService.RefreshTokensAsync(currentRefreshToken);
            SetRefreshToken(refreshedTokens.RefreshToken);
            return Ok(refreshedTokens.JwtToken);
        }
        catch (Exception ex)
        {
            // TODO handle exceptions
            return BadRequest();
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetAllUserRolesAsync))]
    public async Task<ActionResult> GetAllUserRolesAsync()
    {
        try
        {
            var identityIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            var userRoleDtos = await _identityManagmentService.GetAllUserRolesAsync(identityIdClaim?.Value);
            return Ok(userRoleDtos);
        }
        catch (ArgumentNullException ex)
        {
            // TODO handle other exceptions
            return BadRequest(ex.Message);
        }
    }

    private void SetRefreshToken(RefreshTokenDto refreshToken)
    {
        var cookieOptions = new CookieOptions
        { 
            HttpOnly = true,
            Expires = refreshToken.Expires
        };

        Response.Cookies.Append(RefreshTokenKeyWord, refreshToken.Token, cookieOptions);
    }

    private void DeleteRefreshToken()
    {
        Response.Cookies.Delete(RefreshTokenKeyWord);
    }
}
