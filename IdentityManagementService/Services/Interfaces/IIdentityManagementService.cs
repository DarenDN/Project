namespace IdentityManagementService.Services.Interfaces;

using Dtos;

public interface IIdentityManagementService
{
    Task<bool> RegisterUserAsync(RegisterUserDto userAuthDto);

    Task<bool> CreateUserAsync(RegisterUserDto userAuthDto);

    Task<string> LoginAsync(UserAuthDto userAuthDto);

    Task<bool> LogoutAsync();

    Task<bool> DeleteUserAsync(Guid? identityId);

    Task<string> RefreshTokensAsync();

    Task<bool> CreateUserRoleAsync(string? newRoleName);

    Task<bool> DeleteUserRoleAsync(Guid? roleId);

    Task<List<UserRoleDto>> GetAllUserRolesAsync();
}
