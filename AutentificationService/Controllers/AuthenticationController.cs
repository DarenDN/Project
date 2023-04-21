namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos;
using IdentityManagementService.Services.Interfaces;

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
    [Route(nameof(RegisterUserAsync))]
    public async Task<ActionResult<object>> RegisterUserAsync(UserAuthDto userAuthDto)
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

    // TODO if the methode is correct
    // TODO delete can only an admin/manager and only authorized one
    [HttpDelete]
    [Route(nameof(DeleteUserAsync))]
    public async Task<ActionResult<object>> DeleteUserAsync(Guid userAuthDto)
    {
        try
        {
            var deletingResult = await _identManagmentService.DeleteUserAsync(userAuthDto);
            return Ok(deletingResult);
        }
        catch (Exception ex)
        {
            // TODO difs exceptions
            return BadRequest(ex.Message);
        }
    }
}
