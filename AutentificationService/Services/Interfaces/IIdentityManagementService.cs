using IdentityManagementService.Dtos;

namespace IdentityManagementService.Services.Interfaces;

public interface IIdentityManagementService
{
    // TODO methode we need
    Task<UserAuthDto> CreateUserAsync(UserAuthDto userAuthDto);

    Task<string> LoginAsync(UserAuthDto userAuthDto);

    Task<bool> DeleteUserAsync(Guid userId);
}
