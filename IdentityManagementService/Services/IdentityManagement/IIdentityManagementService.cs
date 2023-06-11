namespace IdentityManagementService.Services.IdentityManagement;

using Dtos;

public interface IIdentityManagementService
{
    Task<string> RegisterUserIdentityAsync(RegisterIdentityDto userAuthDto);
    Task RegisterUserDataAsync(RegisterUserDataDto userAuthDto);

    Task<Guid> CreateUserAsync(CreateUserDto userAuthDto);

    Task DeleteUserAsync(Guid? identityId);

    Task<IEnumerable<UserIdentityDto>> GetUsersAsync();

    Task<UserIdentityDto> GetUserAsync(Guid? identityId);

    Task<ShortUserInfoDto> GetCurrentShortUserInfoAsync();
    Task<UserIdentityDto> GetCurrentFullUserInfoAsync();
    Task<ShortUserInfoDto> GetShortUserInfoAsync(Guid? identityId = null);
    Task<IEnumerable<ShortUserInfoDto>> GetShortUserInfosAsync(IEnumerable<Guid?> identityIds);

    Task UpdateUserAsync();

    Task SetProjectIdToUserAsync(Guid projectId);
}
