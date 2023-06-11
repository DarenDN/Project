namespace ProjectManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Services.ProductBacklog;

/// <summary>
/// Responsible for dealing with UserStories
/// </summary>
[Route("api/[controller]"), Authorize]
[EnableCors("CorsPolicy")]
[ApiController]
public sealed class UserStoryController : ControllerBase
{
    private readonly IUserStoryService _productBacklogService;

    public UserStoryController(IUserStoryService productBacklogService)
    {
        _productBacklogService = productBacklogService;
    }

    [HttpPost]
    [Route(nameof(CreateUserStoryAsync))]
    public async Task<ActionResult> CreateUserStoryAsync()
    {
        throw new NotImplementedException();
    }
}
