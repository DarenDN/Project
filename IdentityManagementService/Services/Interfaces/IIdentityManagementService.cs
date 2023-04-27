using IdentityManagementService.Dtos;

namespace IdentityManagementService.Services.Interfaces;

public interface IIdentityManagementService
{
    Task<bool> RegisterUserAsync(RegisterUserDto userAuthDto);

    Task<UserAuthDto> CreateUserAsync(RegisterUserDto userAuthDto);

    Task<TokenUserDto> LoginAsync(UserAuthDto userAuthDto);

    Task<bool> LogoutAsync(UserAuthDto userAuthDto);

    Task<bool> DeleteUserAsync(Guid identityId);

    Task<TokenUserDto> RefreshTokensAsync(string refreshToken);

    Task<List<UserRoleDto>> GetAllUserRolesAsync(string identityId);
}
