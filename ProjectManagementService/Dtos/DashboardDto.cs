namespace ProjectManagementService.Dtos;
using Models;
public sealed class DashboardDto
{
    public string Title { get; private set; }
    public Guid ID { get; private set; }

    public DashboardDto(DashboardModel dashboardModel)
    {
        Title = dashboardModel.Title;
        ID = dashboardModel.ID;
    }
}
