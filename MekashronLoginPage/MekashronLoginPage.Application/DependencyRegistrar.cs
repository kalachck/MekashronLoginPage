using MekashronLoginPage.Application.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MekashronLoginPage.Application;

public static class DependencyRegistrar
{
    public static void AddApplicationDependencies(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<LoginConfig>(configuration.GetSection(LoginConfig.SectionName));
        services.AddSingleton<ILoginConfigProvider, LoginConfigProvider>();
        
        services.AddScoped<ILoginProvider, LoginProvider>();

        services.AddHttpClient<LoginProvider>();
    }
}
