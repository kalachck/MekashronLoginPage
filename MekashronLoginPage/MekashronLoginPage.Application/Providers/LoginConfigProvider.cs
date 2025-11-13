using Microsoft.Extensions.Options;

namespace MekashronLoginPage.Application.Providers;

public class LoginConfig
{
    public const string SectionName = "LoginConfig";

    public string SoapUrl { get; init; } = null!;
}

public interface ILoginConfigProvider
{
    string GetSoapUrl();
}

public class LoginConfigProvider : ILoginConfigProvider
{
    private readonly LoginConfig _config;
    
    public LoginConfigProvider(IOptions<LoginConfig> options)
    {
        _config = options.Value;
    }
    
    public string GetSoapUrl()
        => _config.SoapUrl;
}
