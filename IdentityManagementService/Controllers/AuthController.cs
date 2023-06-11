namespace IdentityManagementService.Controllers;

using IdentityManagementService.Dtos;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Services.Auth;

[ApiController]
[EnableCors("CorsPolicy")]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        this._authService = authService;
    }

    [HttpPost]
    [Route(nameof(LoginAsync))]
    public async Task<ActionResult> LoginAsync(UserAuthDto userAuthDto)
    {
        try
        {
            var createdToken = await _authService.LoginAsync(userAuthDto);
            return new JsonResult(new { createdToken });
        }
        catch (ArgumentNullException argNullEx)
        {
            return BadRequest(argNullEx.Message);
        }
        catch (ArgumentException argEx)
        {
            this.HttpContext.Response.StatusCode = 401;
            return new JsonResult(new { argEx.Message });
            //return Unauthorized(argEx.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(LogoutAsync))]
    public async Task<ActionResult> LogoutAsync()
    {
        try
        {
            await _authService.LogoutAsync();
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(RefreshTokenAsync))]
    public async Task<ActionResult> RefreshTokenAsync()
    {
        try
        {
            var newAccessToken = await _authService.RefreshTokensAsync();
            return new JsonResult(new { newAccessToken });
        }
        catch(SecurityTokenExpiredException)
        {
            return Unauthorized();
        }
        catch (Exception ex)
        {
            // TODO handle exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(IsAuthorized))]
    public async Task<ActionResult> IsAuthorized()
    {
        try
        {
            var isAuthorized = await _authService.IsAuthorized();
            return new JsonResult(new { isAuthorized });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
