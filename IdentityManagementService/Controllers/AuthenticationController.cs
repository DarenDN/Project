namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos;
using IdentityManagementService.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private IIdentityManagementService _identManagmentService;

    public AuthenticationController(IIdentityManagementService identManagmentService)
    {    
        _identManagmentService = identManagmentService;
    }

    [HttpPost]
    [Route(nameof(CreateUserAsync))]
    public async Task<ActionResult<object>> CreateUserAsync(UserAuthDto userAuthDto)
    {
        try
        {
            var createdUser = await _identManagmentService.CreateUserAsync(userAuthDto);
            return Ok(createdUser);
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
            await _identManagmentService.LogoutAsync(userAuthDto);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(LoginAsync))]
    public async Task<ActionResult<string>> LoginAsync(UserAuthDto userAuthDto)
    {
        try
        {
            var createdToken = await _identManagmentService.LoginAsync(userAuthDto);
            return Ok(createdToken);
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }

    // TODO delete can only an admin/manager and only authorized one
    [HttpDelete, Authorize(Roles = "")]
    [Route(nameof(DeleteUserAsync))]
    public async Task<ActionResult> DeleteUserAsync(Guid userAuthDto)
    {
        try
        {
             await _identManagmentService.DeleteUserAsync(userAuthDto);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }
}
