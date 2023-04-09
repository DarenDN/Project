namespace AutentificationService.Controllers;

using AutentificationService.Dtos;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthenticationController : ControllerBase
{
    private AuthenticationService _authService;

    public AuthenticationController(AuthenticationService authService)
    {
        _authService = authService;
    }


    [HttpPost]
    [Route(nameof(CreateUser))]
    public async Task<JsonResult> CreateUser(UserAuthDto userAuthDto)
    {
        // TODO add service call
        return View();
    }
}
