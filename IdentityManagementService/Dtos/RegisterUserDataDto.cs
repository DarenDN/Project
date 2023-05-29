namespace IdentityManagementService.Dtos;

public record RegisterUserDataDto(
    string FirstName,
    string LastName,
    string? MiddleName,
    string? Email,
    Guid ProjectId,
    Guid RoleId);
