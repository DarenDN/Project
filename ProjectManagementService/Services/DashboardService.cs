namespace ProjectManagementService.Services;
using Microsoft.AspNetCore.Mvc;
using Data;
using Dtos;
public sealed class DashboardService
{
    private readonly ApplicationDbContext _applicationDbContext;
    public DashboardService(ApplicationDbContext appDbContext)
    {
        _applicationDbContext = appDbContext;
    }

    public async Task<IEnumerable<DashboardDto>> GetProjectDashboardsByUserId(Guid projectID)
    {
        var dashboards = _applicationDbContext.DashboardModels.Where(d => d.Project.ID == projectID);
        var dashboardDtos = dashboards.Select(d => new DashboardDto(d));
        return dashboardDtos;
    }
}
