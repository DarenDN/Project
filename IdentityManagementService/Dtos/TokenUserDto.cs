namespace IdentityManagementService.Dtos;

public record TokenUserDto(string JwtToken, RefreshTokenDto RefreshToken);
