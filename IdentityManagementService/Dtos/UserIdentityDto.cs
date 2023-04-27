namespace IdentityManagementService.Dtos;

public sealed record UserIdentityDto (
    Guid IdentityId, 
    string Login, 
    string FirstName, 
    string LastName, 
    string MiddleName, 
    string Email,
    DateTime Registered,
    string Role);
