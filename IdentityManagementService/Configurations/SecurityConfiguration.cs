namespace IdentityManagementService.Configurations;

public sealed class SecurityConfiguration
{
    public string Token { get; set; }
    public string AccessTokenLifeTime { get; set; }
    public string RefreshTokenLifeTime { get; set; }
}
