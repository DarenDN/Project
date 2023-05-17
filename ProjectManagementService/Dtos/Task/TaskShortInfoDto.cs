namespace ProjectManagementService.Dtos.Task;
public sealed record TaskShortInfoDto(Guid Id, string Title, string Status, string Type, Guid? Actor, Guid Creator);
