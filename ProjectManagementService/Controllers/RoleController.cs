namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Dtos.Role;
using Services.Role;
using Microsoft.AspNetCore.Authorization;

// working with project could be easily made into one own microservice
[ApiController, Authorize]
[EnableCors("CorsPolicy")]
[Route("api/[controller]")]
public class RoleController : ControllerBase
{
    private IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }

    [HttpPost]
    [Route(nameof(CreateRoleAsync))]
    public async Task<ActionResult> CreateRoleAsync(CreateRoleDto createRoleDto)
    {
        try
        {
            await _roleService.CreateRoleAsync(createRoleDto);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(GetRolesAsync))]
    public async Task<ActionResult> GetRolesAsync()
    {
        try
        {
            var roles = await _roleService.GetRolesAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            // TODO exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost]
    [Route(nameof(GetRoleAsync))]
    public async Task<ActionResult> GetRoleAsync(Guid roleId)
    {
        try
        {
            var role = await _roleService.GetRoleAsync(roleId);
            return Ok(role);
        }
        catch (Exception ex)
        {
            // TODO exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpDelete]
    [Route(nameof(DeleteRoleAsync))]
    public async Task<ActionResult> DeleteRoleAsync(Guid roleId)
    {
        try
        {
            await _roleService.DeleteRoleAsync(roleId);
            return Ok();
        }
        catch (Exception ex)
        {
            // TODO exceptions
            return StatusCode(500, ex.Message);
        }
    }

    [HttpGet]
    [Route(nameof(IsAdminAsync))]
    public async Task<ActionResult> IsAdminAsync()
    {
        try
        {
            var isAdmin = await _roleService.IsAdminAsync();
            return Ok(isAdmin);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
