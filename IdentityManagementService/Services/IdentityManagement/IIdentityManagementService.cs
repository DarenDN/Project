namespace IdentityManagementService.Services.IdentityManagement;

using Dtos;

public interface IIdentityManagementService
{
    Task RegisterUserAsync(RegisterUserDto userAuthDto);

    Task CreateUserAsync(CreateUserDto userAuthDto);

    Task DeleteUserAsync(Guid? identityId);

    Task<IEnumerable<UserDto>> GetUsers();

    Task<UserIdentityDto> GetUser(Guid? identityId);
}
