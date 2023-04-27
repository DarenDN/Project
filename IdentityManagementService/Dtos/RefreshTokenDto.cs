namespace IdentityManagementService.Dtos;

public record RefreshTokenDto(string Token, DateTime Expires);
