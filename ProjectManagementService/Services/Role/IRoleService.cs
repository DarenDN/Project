namespace ProjectManagementService.Services.Role;

using Dtos.Role;
using Task = System.Threading.Tasks.Task;

public interface IRoleService
{
    Task CreateRoleAsync(CreateRoleDto roleDto);

    Task DeleteRoleAsync(Guid roleId);

    Task<IEnumerable<RoleDto>> GetRolesAsync();

    Task<RoleDto> GetRoleAsync(Guid roleId);

    Task UpdateRoleAsync(RoleDto roleDto);

    Task<bool> IsAdminAsync();
}
