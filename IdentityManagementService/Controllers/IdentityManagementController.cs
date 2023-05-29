namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos;
using Services.IdentityManagement;

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
    [Route(nameof(RegisterIdentityAsync))]
    public async Task<ActionResult> RegisterIdentityAsync(RegisterIdentityDto registerIdentityDto)
    {
        try
        {
            var accessToken = await _identityManagmentService.RegisterUserIdentityAsync(registerIdentityDto);
            return Ok(accessToken);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost, Authorize]
    [Route(nameof(RegisterDataAsync))]
    public async Task<ActionResult> RegisterDataAsync(RegisterUserDataDto registerUserDto)
    {
        try
        {
            await _identityManagmentService.RegisterUserDataAsync(registerUserDto);
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost, Authorize]
    [Route(nameof(CreateUserAsync))]
    public async Task<ActionResult> CreateUserAsync(CreateUserDto registerUserDto)
    {
        try
        {
            var identityId = await _identityManagmentService.CreateUserAsync(registerUserDto);
            return Ok(identityId);
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return StatusCode(500, ex.Message);
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
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetUsersAsync))]
    public async Task<ActionResult> GetUsersAsync()
    {
        try
        {
            var users = await _identityManagmentService.GetUsersAsync();
            return Ok(users);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetUserAsync))]
    public async Task<ActionResult> GetUserAsync()
    {
        try
        {
            var user = await _identityManagmentService.GetUserAsync();
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
