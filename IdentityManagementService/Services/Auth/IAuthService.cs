namespace IdentityManagementService.Services.IdentityManagement;

using Dtos;

public interface IAuthService
{
    Task<string> LoginAsync(UserAuthDto userAuthDto);

    Task LogoutAsync();

    Task<string> RefreshTokensAsync();

    Task<bool> IsAuthorized();
}
