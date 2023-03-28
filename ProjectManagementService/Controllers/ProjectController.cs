namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : ControllerBase
{
    [HttpGet]
    [Route(nameof(GetProjects))]
    public async Task<JsonResult> GetProjects()
    {
        // TODO implementation
        // 1st is handler

        return new JsonResult(1);
    }
}
