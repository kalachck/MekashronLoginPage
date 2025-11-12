using Microsoft.Extensions.Options;

namespace MekashronLoginPage.Application;

public class AuthorizationConfig
{
    public const string SectionName = "AuthConfig";

    public string AuthUrl { get; set; } = null!;
}

public interface IAuthorizationConfigProvider
{
    string GetAuthUrl();
}

public class AuthorizationConfigProvider : IAuthorizationConfigProvider
{
    private readonly AuthorizationConfig _config;
    
    public AuthorizationConfigProvider(IOptions<AuthorizationConfig> options)
    {
        _config = options.Value;
    }
    
    public string GetAuthUrl()
        => _config.AuthUrl;
}
