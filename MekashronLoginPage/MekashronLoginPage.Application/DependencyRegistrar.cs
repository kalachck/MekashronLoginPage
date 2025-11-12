using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MekashronLoginPage.Application;

public static class DependencyRegistrar
{
    public static void AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AuthorizationConfig>(configuration.GetSection(AuthorizationConfig.SectionName));
        services.AddSingleton<IAuthorizationConfigProvider, AuthorizationConfigProvider>();
        
        services.AddScoped<IAuthorizationProvider, AuthorizationProvider>();
    }
}
