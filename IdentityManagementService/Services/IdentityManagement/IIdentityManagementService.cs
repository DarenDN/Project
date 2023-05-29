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

    Task<CurrentUserInfoDto> GetUserAsync();

    Task UpdateUserAsync();

    Task SetProjectIdToUserAsync(Guid projectId);
}
