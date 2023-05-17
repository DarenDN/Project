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
    [Route(nameof(RegisterAsync))]
    public async Task<ActionResult> RegisterAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            await _identityManagmentService.RegisterUserAsync(registerUserDto);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO ex
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost, Authorize]
    [Route(nameof(CreateUserAsync))]
    public async Task<ActionResult> CreateUserAsync(RegisterUserDto registerUserDto)
    {
        try
        {
            await _identityManagmentService.CreateUserAsync(registerUserDto);
            return Ok();
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
}
