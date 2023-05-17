namespace ProjectManagementService.Dtos.Dashboard;

public record CreateDashboardDto(string Title, string? Description, List<Guid>? AllowedRoles);
