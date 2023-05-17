namespace IdentityManagementService.Dtos;

public sealed record UserIdentityDto (
    Guid IdentityId,
    Guid? Role,
    string Login, 
    string FirstName, 
    string LastName, 
    string? MiddleName, 
    string Email,
    DateTime Registered);
