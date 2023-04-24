namespace SecurityLibrary.JwtAuthenticationConfiguration;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

public static class JwtAuthenticationConfiguration
{
    private const string SecurityCfgFileName = "SecurityConfiguration";
    private const string SecurityCfgTokenSection = "SecurityConfiguration:Token";

    public static void ConfigureJwtAuthentication(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        configurationManager.AddJsonFile(SecurityCfgFileName);

        serviceCollection.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            opt => opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.Unicode.GetBytes(
                        configurationManager.GetValue<string>(SecurityCfgTokenSection))),
                ValidateIssuer = false,
                ValidateAudience = false
            });
    }

    public static void ConfigureAppAuthentication(this WebApplication webApplication)
    {
        webApplication.UseAuthentication();
        webApplication.UseAuthorization();
    }
}