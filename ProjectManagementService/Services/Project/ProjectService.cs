namespace ProjectManagementService.Services.Project;

using Microsoft.EntityFrameworkCore;
using Data;
using Dtos.Project;
using Models;
using System.Security.Claims;
using Task = System.Threading.Tasks.Task;

public sealed class ProjectService : IProjectService
{
    private readonly ApplicationDbContext _appDbContext;
    private IHttpContextAccessor _httpContextAccessor;

    public ProjectService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<ProjectDto> GetProjectAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        return new ProjectDto(projectId.Value, project.Title);
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

    public async Task CreateProjectAsync(CreateProjectDto projectDto)
    {
        var newProject = new Project
        {
            Title = projectDto.Title,
            Description = projectDto.Description,
        };

        await _appDbContext.AddAsync(newProject);
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

    private async Task ChainIdentityToProjectAsync(Guid projectId, Guid identityId)
    {
        var projectsIdentity = new ProjectsIdentity
        {
            ProjectId = projectId,
            IdentityId = identityId
        };

        await _appDbContext.ProjectsIdentities.AddAsync(projectsIdentity);
        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch
        {
            throw;
        }
    }

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
}
