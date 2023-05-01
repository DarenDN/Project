namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos;
using Services.Interfaces;   

[ApiController]
[Route("api/[controller]")]
public sealed class IdentityManagementController : ControllerBase
{
    private IIdentityManagementService _identityManagmentService;

    public IdentityManagementController(IIdentityManagementService identManagmentService)
    {
        _identityManagmentService = identManagmentService;
    }


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

    [HttpPost]
    [Route(nameof(LoginAsync))]
    public async Task<ActionResult> LoginAsync(UserAuthDto userAuthDto)
    {
        try
        {
            var createdToken = await _identityManagmentService.LoginAsync(userAuthDto);
            return Ok(createdToken);
        }
        catch (ArgumentNullException argNullEx)
        {
            return BadRequest(argNullEx.Message);
        }
        catch (ArgumentException argEx)
        {
            return Unauthorized(argEx.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500 ,ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(LogoutAsync))]
    public async Task<ActionResult> LogoutAsync()
    {
        try
        {
            await _identityManagmentService.LogoutAsync();
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
    [Route(nameof(RefreshTokenAsync))]
    public async Task<ActionResult> RefreshTokenAsync()
    {
        try
        {
            var newAccessToken = await _identityManagmentService.RefreshTokensAsync();
            return Ok(newAccessToken);
        }
        catch (Exception ex)
        {
            // TODO handle exceptions
            return BadRequest();
        }
    }

    [HttpPost, Authorize]
    [Route(nameof(CreateUserRoleAsync))]
    public async Task<ActionResult> CreateUserRoleAsync(string newRoleName)
    {
        try
        {
            await _identityManagmentService.CreateUserRoleAsync(newRoleName);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO
            return BadRequest();
        }
    }

    [HttpDelete, Authorize]
    [Route(nameof(DeleteUserRoleAsync))]
    public async Task<ActionResult> DeleteUserRoleAsync(Guid roleId)
    {
        try
        {
            await _identityManagmentService.DeleteUserRoleAsync(roleId);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO
            return BadRequest();
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetAllUserRolesAsync))]
    public async Task<ActionResult> GetAllUserRolesAsync()
    {
        try
        {
            var userRoleDtos = await _identityManagmentService.GetAllUserRolesAsync();
            return Ok(userRoleDtos);
        }
        catch (ArgumentNullException ex)
        {
            // TODO handle other exceptions
            return BadRequest(ex.Message);
        }
    }
}
