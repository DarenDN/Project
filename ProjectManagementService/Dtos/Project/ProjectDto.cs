namespace ProjectManagementService.Dtos.Project;
public sealed record ProjectDto(Guid Id, string Title, string? Description, DateTime? DateCreated);
