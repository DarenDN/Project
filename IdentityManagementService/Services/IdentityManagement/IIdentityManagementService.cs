namespace IdentityManagementService.Services.IdentityManagement;

using Dtos;

public interface IIdentityManagementService
{
    Task<string> RegisterUserIdentityAsync(RegisterIdentityDto userAuthDto);
    Task RegisterUserDataAsync(RegisterUserDataDto userAuthDto);

    Task<Guid> CreateUserAsync(CreateUserDto userAuthDto);

    Task DeleteUserAsync(Guid? identityId);

    Task<IEnumerable<UserDto>> GetUsersAsync();

    Task<UserIdentityDto> GetUserAsync(Guid? identityId);

    Task<ShortUserInfoDto> GetUserAsync();
    Task<ShortUserInfoDto> GetShortUserInfoAsync(Guid? identityId = null);
    Task<IEnumerable<ShortUserInfoDto>> GetShortUserInfosAsync(IEnumerable<Guid?> identityIds);

    Task UpdateUserAsync();

    Task SetProjectIdToUserAsync(Guid projectId);
}
