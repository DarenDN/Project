namespace ProjectManagementService.Services.Dashboard;

using Microsoft.EntityFrameworkCore;
using Data;
using Models;
using System.Collections.Generic;
using System.Security.Claims;
using ProjectManagementService.Dtos.Dashboard;

public sealed class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _appDbContext;
    private readonly ILogger _logger;
    private IHttpContextAccessor _httpContextAccessor;

    public DashboardService(ApplicationDbContext appDbContext, IHttpContextAccessor httpContextAccessor)
    {
        _appDbContext = appDbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> CreateDashboardAsync(CreateDashboardDto dashboardDto)
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(p => p.Id == projectId);

        var newDashboard = new Dashboard
        {
            Title = dashboardDto.Title,
            Description = dashboardDto.Description,
            AllowedUserRoles = dashboardDto.AllowedRoles,
            Project = project
        };

        await _appDbContext.Dashboards.AddAsync(newDashboard);

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // TODO exceptions
            throw;
        }

        return true;
    }

    public async Task<bool> UpdateDashboardAsync(UpdateDashboardDto dashboardDto)
    {
        var dashboard = await _appDbContext.Dashboards.FirstOrDefaultAsync(d => d.Id == dashboardDto.Id);

        dashboard.Title = dashboardDto.Title;
        dashboard.Description = dashboardDto.Description;
        dashboard.AllowedUserRoles = dashboardDto.AllowedUserRoles;

        _appDbContext.Dashboards.Update(dashboard);

        try
        {
            await _appDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {

        }

        return true;
    }

    public async Task<bool> DeleteDashboardAsync(Guid dashboardId)
    {
        throw new NotImplementedException();
        //var dashboardToDelete = await _appDbContext.Dashboards.FirstOrDefaultAsync(p => p.Id == dashboardId);
        //var tasksToDelete = await _appDbContext.Tasks.Where(t => t.DashboardId == dashboardId).ToListAsync();

        //_appDbContext.Tasks.RemoveRange(tasksToDelete);
        //_appDbContext.Dashboards.Remove(dashboardToDelete);

        //try
        //{
        //    await _appDbContext.SaveChangesAsync();
        //}
        //catch (Exception ex)
        //{
        //    // TODO exceptions
        //    throw;
        //}

        //return true;
    }

    public async Task<DashboardDto> GetDashboardAsync(Guid dashboardId)
    {
        var dashboard = await _appDbContext.Dashboards.FirstOrDefaultAsync(d => d.Id == dashboardId);
        // TODO handle null result
        // TODO should be something like DashboardDataDto
        var dashboardDto = new DashboardDto(dashboard.Id, dashboard.Title, dashboard.Description);
        return dashboardDto;
    }

    public async Task<List<DashboardDto>> GetDashboardsAsync()
    {
        var projectId = await GetRequestingUsersProjectIdAsync();
        if (projectId is null)
        {
            throw new ArgumentNullException(nameof(projectId));
        }

        var dashboards = await _appDbContext.Dashboards
            .Include(d => d.Project)
            .Where(d => d.Project.Id == projectId)
            .Select(d => new DashboardDto(d.Id, d.Title, d.Description))
            .ToListAsync();

        return dashboards;
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
}
