namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Type;

[ApiController, Authorize]
[Route("api/[controller]")]
public class TypeController : ControllerBase
{
    private readonly ITypeService _typeService;
    public TypeController(ITypeService typeService)
    {
        _typeService = typeService;
    }

    [HttpGet]
    [Route(nameof(GetTypesAsync))]
    public async Task<ActionResult> GetTypesAsync()
    {
        try
        {
            var types = await _typeService.GetTypesAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}
