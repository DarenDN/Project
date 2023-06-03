namespace ProjectManagementService.Services.Project;

using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Project;
using Models;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;
using System;
using Microsoft.Extensions.Options;
using ProjectManagementService.Configurations;

public sealed class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _appDbContext;
    private IHttpContextAccessor _httpContextAccessor;
    private readonly RoleConfiguration _roleConfiguration;

    public ProjectService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor, IOptions<RoleConfiguration> options)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
        _roleConfiguration = options.Value;
    }   

    public async Task<ProjectDto> GetProjectAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        return new ProjectDto(projectId.Value, project.Title, project.Description);
    }

    public async Task UpdateProjectAsync(UpdateProjectDto projectDto)
    {
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(d => d.Id == projectDto.Id);

        project.Title = projectDto.Title;
        project.Description = projectDto.Description;

        _appDbContext.Projects.Update(project);

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<CreatedProjectDto> CreateProjectAsync(CreateProjectDto projectDto)
    {
        var newProject = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
        };

        await _appDbContext.Projects.AddAsync(newProject);
        var identityId = GetRequestingIdentityId();
        if(identityId is null)
        {
            throw new ArgumentException(nameof(identityId));
        }

        await ChainIdentityToProjectAsync(newProject.Id, identityId.Value);
        await SetUpBaseRolseAsync(newProject.Id);
        var adminRoleId = await GetAdminRoleAsync();

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO exceptions
            throw;
        }

        return new CreatedProjectDto(newProject.Id, adminRoleId.Id);
    }

    public async Task DeleteProjectAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var projectToDelete = await _appDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        var deletedProject = _appDbContext.Projects.Remove(projectToDelete);

        // TODO delete all coresponding info, including users, tasks, dashboards and etc
        // Also, log out users, or show them message "project was deleted" slt

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO exceptions
            throw;
        }
    }

    private async Task SetUpBaseRolseAsync(Guid projectId)
    {
        foreach(var role in _roleConfiguration.BasicRoles)
        {
            var roleId = role.Value;
            if(!await _appDbContext.ProjectsRoles.AnyAsync(pr => pr.ProjectId == projectId && pr.RoleId == roleId))
            {
                await _appDbContext.ProjectsRoles.AddAsync(new ProjectsRole {ProjectId = projectId, RoleId = roleId });
            }
        }
    }

    /// <summary>
    /// Doesnt save changes.
    /// </summary>
    /// <param name="projectId"></param>
    /// <param name="identityId"></param>
    /// <returns></returns>
    private async Task ChainIdentityToProjectAsync(Guid projectId, Guid identityId)
    {
        var projectsIdentity = new ProjectsIdentity
        {
            ProjectId = projectId,
            IdentityId = identityId
        };

        await _appDbContext.ProjectsIdentities.AddAsync(projectsIdentity);
    }

    // TODO move method. Wrong class logic.
    /// <summary>
    /// Doesnt save changes.
    /// Creates Admin role if not exists and creates ProjectRole
    /// </summary>
    /// <param name="projectId"></param>
    /// <returns></returns>

    private Guid? GetRequestingIdentityId()
    {
        var identityIdString = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return Guid.TryParse(identityIdString, out var identityId)
                            ? identityId
                            : null;
    }

    private async Task<Guid?> GetRequestingUsersProjectIdAsync()
    {
        var identityId = GetRequestingIdentityId();
        if (identityId is null)
        {
            return null;
        }

        var projectsIdentity = await _appDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == identityId);

        return projectsIdentity?.ProjectId;
    }

    public async Task SetProjectIdentityAsync(Guid identityId)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if(projectId is null)
        {
            throw new ArgumentException();
        }

        if(await _appDbContext.ProjectsIdentities.AnyAsync(pi => pi.IdentityId == identityId && pi.ProjectId == projectId))
        {
            return;
        }

        var projectsIdentity = new ProjectsIdentity { IdentityId = identityId, ProjectId = projectId.Value };
        _appDbContext.ProjectsIdentities.Add(projectsIdentity);

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }

    private async Task<UserRole?> GetAdminRoleAsync()
    {
        if(!_roleConfiguration.BasicRoles.TryGetValue(_roleConfiguration.AdminRole, out var adminRoleId))
        {
            return null;
        }

        return await _appDbContext.UserRoles.FirstOrDefaultAsync(r => r.Id == adminRoleId);
    }

    public async Task DeleteProjectIdentityAsync(Guid identityId)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if (projectId is null)
        {
            throw new ArgumentException();
        }
        var projectsIdentity = await _appDbContext.ProjectsIdentities.FirstOrDefaultAsync(pi => pi.IdentityId == identityId && pi.ProjectId == projectId);

        if (projectsIdentity is null)
        {
            return;
        }

        _appDbContext.ProjectsIdentities.Remove(projectsIdentity);

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }
}
