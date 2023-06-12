namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Dtos;
using Services.IdentityManagement;
using Microsoft.AspNetCore.Cors;
using IdentityManagementService.Models;

[ApiController]
[EnableCors("CorsPolicy")]
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
            return new JsonResult(new { accessToken });
        }
        catch(ArgumentException argEx)
        {
            this.HttpContext.Response.StatusCode = 400;
            return new JsonResult(new { argEx.Message });
            //return Unauthorized(argEx.Message);
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
            return new JsonResult(new { identityId });
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
            var userIdentityDtos = await _identityManagmentService.GetUsersAsync();
            return new JsonResult(new { userIdentityDtos });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut, Authorize]
    [Route(nameof(GetUserInfoAsync))]
    public async Task<ActionResult> GetUserInfoAsync(Guid? identityId)
    {
        try
        {
            var userIdentityDto = await _identityManagmentService.GetUserAsync(identityId);
            return new JsonResult(userIdentityDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut, Authorize]
    [Route(nameof(GetShortUserInfoAsync))]
    public async Task<ActionResult> GetShortUserInfoAsync(Guid? identityId)
    {
        try
        {
            var shortUserInfoDto = await _identityManagmentService.GetShortUserInfoAsync(identityId);
            return new JsonResult(shortUserInfoDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetCurrentShortUserInfoAsync))]
    public async Task<ActionResult> GetCurrentShortUserInfoAsync()
    {
        try
        {
            var shortUserInfoDto = await _identityManagmentService.GetCurrentShortUserInfoAsync();
            return new JsonResult(shortUserInfoDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet, Authorize]
    [Route(nameof(GetCurrentFullUserInfoAsync))]
    public async Task<ActionResult> GetCurrentFullUserInfoAsync()
    {
        try
        {
            var userIdentityDto = await _identityManagmentService.GetCurrentFullUserInfoAsync();
            return new JsonResult(userIdentityDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPut, Authorize]
    [Route(nameof(GetShortUserInfosAsync))]
    public async Task<ActionResult> GetShortUserInfosAsync(List<Guid?>? identityIds)
    {
        try
        {
            var shortUserInfoDto = await _identityManagmentService.GetShortUserInfosAsync(identityIds);
            return new JsonResult(shortUserInfoDto);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
