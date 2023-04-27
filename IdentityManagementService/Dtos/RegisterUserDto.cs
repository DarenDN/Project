namespace IdentityManagementService.Dtos;

public record RegisterUserDto(
    string Login,
    string Password,
    string FirstName,
    string LastName,
    string Email,
    Guid ProjectId,
    Guid RoleId,
    string MiddleName = "");
