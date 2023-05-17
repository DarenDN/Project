namespace IdentityManagementService.Dtos;

public record UserDto (Guid IdentityId, Guid? RoldeId, string FirstName, string SecondName, string? MiddleName = null);
