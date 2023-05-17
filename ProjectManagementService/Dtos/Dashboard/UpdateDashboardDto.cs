namespace ProjectManagementService.Dtos.Dashboard;

public record UpdateDashboardDto(Guid Id, string Title, string? Description, List<Guid>? AllowedUserRoles);
