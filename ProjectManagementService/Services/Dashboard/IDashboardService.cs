namespace ProjectManagementService.Services.Dashboard;

using ProjectManagementService.Dtos.Dashboard;

public interface IDashboardService
{
    Task<DashboardDto> GetDashboardAsync(Guid dashboardDto);

    Task<List<DashboardDto>> GetDashboardsAsync();

    Task<bool> CreateDashboardAsync(CreateDashboardDto dashboardDto);

    Task<bool> UpdateDashboardAsync(UpdateDashboardDto dashboardDto);

    Task<bool> DeleteDashboardAsync(Guid dashboardId);
}
