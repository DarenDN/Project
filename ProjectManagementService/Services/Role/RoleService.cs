namespace ProjectManagementService.Services.Role;

using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Data;
using Models;
using Dtos.Role;
using Task = System.Threading.Tasks.Task;
using System.Security.Claims;

public class RoleService : IRoleService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ApplicationDbContext _appDbContext;

    public RoleService(IHttpContextAccessor httpContextAccessor, ApplicationDbContext appDbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _appDbContext = appDbContext;
    }

    public async Task CreateRoleAsync(CreateRoleDto roleDto)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var role = await _appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Name == roleDto.Name);
        if (role != null)
        {
            var existsInProject = await _appDbContext.ProjectsRoles
                .Where(r => r.ProjectId == projectId)
                .AnyAsync(r => r.RoleId == role.Id);

            if (existsInProject)
            {
                throw new ArgumentException(nameof(roleDto));
            }

            var projectRole = new ProjectsRole
            {
                ProjectId = projectId.Value,
                RoleId = role.Id
            };

            await _appDbContext.ProjectsRoles.AddAsync(projectRole);
        }
        else
        {
            var newRole = new UserRole
            {
                Name = roleDto.Name
            };

            var newProjectRole = new ProjectsRole
            {
                ProjectId = projectId.Value,
                RoleId = newRole.Id
            };

            await _appDbContext.UserRoles.AddAsync(newRole);
            await _appDbContext.ProjectsRoles.AddAsync(newProjectRole);
        }

        await TrySaveChangesAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task<IEnumerable<RoleDto>> GetRolesAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();

        if (projectId is null)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        var roles = await _appDbContext.UserRoles
            .Join(_appDbContext.ProjectsRoles.Where(r => r.ProjectId == projectId),
                userRole => userRole.Id,
                projRole => projRole.RoleId,
                (userRole, projRole) => userRole)
            .Distinct()
            .Select(r => new RoleDto(r.Id, r.Name))
            .ToListAsync();

        return roles;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="roleId"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task DeleteRoleAsync(Guid roleId)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if (projectId is null)
        {
            throw new ArgumentNullException();
        }

        var roleInProjectsCount = await _appDbContext.ProjectsRoles.CountAsync(r => r.RoleId == roleId);

        if (roleInProjectsCount > 1)
        {
            var projectRole = await _appDbContext.ProjectsRoles.FirstOrDefaultAsync(r => r.ProjectId == projectId
            && r.RoleId == roleId);

            _appDbContext.ProjectsRoles.Remove(projectRole);
        }
        else
        {
            var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Id == roleId);
            var projectRole = await _appDbContext.ProjectsRoles.FirstOrDefaultAsync(r => r.ProjectId == projectId
            && r.RoleId == roleId);

            _appDbContext.UserRoles.Remove(userRole);
            _appDbContext.ProjectsRoles.Remove(projectRole);
        }

        await TrySaveChangesAsync();
    }

    public async Task<RoleDto> GetRoleAsync(Guid roleId)
    {
        var role = await _appDbContext.UserRoles.FirstOrDefaultAsync(r=>r.Id == roleId);
        return new RoleDto(role.Id, role.Name);
    }

    public async Task UpdateRoleAsync(RoleDto roleDto)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();

        var roleInProjectsCount = await _appDbContext.ProjectsRoles.CountAsync(r => r.RoleId == roleDto.Id);
        var projectRole = await _appDbContext.ProjectsRoles.FirstOrDefaultAsync(r => r.ProjectId == projectId
        && r.RoleId == roleDto.Id);

        if (roleInProjectsCount > 1)
        {
            _appDbContext.ProjectsRoles.Remove(projectRole);

            await CreateRoleAsync(new CreateRoleDto(roleDto.Name));

            return;
        }

        var userRole = await _appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Id == roleDto.Id);
        userRole.Name = roleDto.Name;
        _appDbContext.UserRoles.Update(userRole);

        await TrySaveChangesAsync();
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(identityId))
        {
            return null;
        }

        var projectsIdentity = await _appDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == Guid.Parse(identityId));

        return projectsIdentity?.ProjectId;
    }

    private async Task TrySaveChangesAsync()
    {
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
