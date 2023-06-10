namespace IdentityManagementService.Controllers;

using Microsoft.AspNetCore.Mvc;
using Data;
using IdentityManagementService.Configurations;
using IdentityManagementService.Handler.Hash;
using IdentityManagementService.Models;
using Microsoft.Extensions.Options;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
	private readonly AppDbContext _context;
    private TestUsersConfiguration _testUsersConfiguration;
    private HashHandler _hashHandler;

    public TestController(
        IOptions<TestUsersConfiguration> options,
        AppDbContext identityManagementDbContext)
	{
        _testUsersConfiguration = options.Value;
		_context = identityManagementDbContext;
        _hashHandler = new HashHandler();

    }

    [HttpPost]
    [Route(nameof(FillTestDataAsync))]
    public async Task<ActionResult> FillTestDataAsync()
    {
		try
		{
			await TryFillTestDataAsync();
			return Ok();

        }
		catch (Exception ex)
		{
			return StatusCode(500, ex.Message);
        }
    }

    private async Task TryFillTestDataAsync()
	{
        var identities = new List<Identity>();
        var userInfos = new List<UserInfo>();

        foreach (var testUser in _testUsersConfiguration.TestUsers)
        {
            var userInfo = new UserInfo
            {
                FirstName = testUser.FirstName,
                LastName = testUser.LastName,
                MiddleName = testUser.MiddleName,
                ProjectId = testUser.ProjectId,
                RoleId = testUser.RoleId,
            };
            var (hash, salt) = _hashHandler.CreateHashAndSaltAsync(testUser.Password).Result;
            var identity = new Identity
            {
                Id = testUser.Id,
                Login = testUser.Login,
                PasswordHash = hash,
                PasswordSalt = salt,
                UserInfo = userInfo,
            };

            identities.Add(identity);
            userInfos.Add(userInfo);
        }

        await _context.UserInfos.AddRangeAsync(userInfos);
        await _context.Identities.AddRangeAsync(identities);
        await _context.SaveChangesAsync();
    }
}
