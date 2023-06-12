namespace IdentityManagementService.Dtos;

public record ShortUserInfoDto(Guid Id, string FirstName, string LastName, string? MiddleName);
