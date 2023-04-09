namespace ProjectManagementService.Dtos;
using Models;
public sealed class DashboardDto
{
    public string Title { get; private set; }
    public Guid Id { get; private set; }

    public DashboardDto(Dashboard dashboardModel)
    {
        Title = dashboardModel.Title;
        Id = dashboardModel.Id;
    }
}
