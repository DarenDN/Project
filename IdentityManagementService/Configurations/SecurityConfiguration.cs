namespace IdentityManagementService.Configurations;

public sealed class SecurityConfiguration
{
    public static string ConfigurationName = "SecurityConfiguration";
    public string Token { get; set; }
    public string AccessTokenLifeTime { get; set; }
    public string RefreshTokenLifeTime { get; set; }
}
