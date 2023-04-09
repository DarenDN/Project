namespace ProjectManagementService.Services;

using Data;
using Dtos;
using Microsoft.EntityFrameworkCore;
using ProjectManagementService.Models;

public sealed class DashboardService
{
    private readonly ApplicationDbContext _appDbContext;
    public DashboardService(ApplicationDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task<List<TaskDto>> GetTasksAsync(Guid dashboardId)
    {
        var tasks = _appDbContext.Tasks
                                .Where(t => t.Dashboard.Id == dashboardId)
                                .Select(t=>new TaskDto(t));

        return await tasks.ToListAsync();
    }

    public async Task<DashboardDto> CreateDashboardAsync(DashboardDto dashboardDto)
    {
        var newDashboard = new Dashboard
        {
            // TODO fill in
        };

        var createdEntity = await _appDbContext.AddAsync(newDashboard);
        // TODO check if saved

        await _appDbContext.SaveChangesAsync();
        return dashboardDto;
    }

    public async Task<DashboardDto> UpdateDashboardAsync(DashboardDto dashboardDto)
    {
        var project = await _appDbContext.Projects.FirstOrDefaultAsync(d => d.Id == dashboardDto.Id);

        
        // TODO change data to dto
        // TODO need to check result everywhere
        await _appDbContext.SaveChangesAsync();

        return dashboardDto;
    }

    public async Task<bool> DeleteDashboardAsync(Guid dashboardId)
    {
        var dashboardToDelete = await _appDbContext.Dashboards.FirstOrDefaultAsync(p => p.Id == dashboardId);
        // TODO for some reason if null?

        var deletedEntity = _appDbContext?.Remove(dashboardToDelete);
        if (deletedEntity is null)
        {
            return false;
        }
        // TODO delete all coresponding info, including users, tasks, dashboards and etc
        // Also, log out users, or show them message "project was deleted" slt
        await _appDbContext.SaveChangesAsync();

        return true;
    }
}
