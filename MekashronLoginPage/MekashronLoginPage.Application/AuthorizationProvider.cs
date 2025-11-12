using System.Net;
using System.Net.Mime;
using System.Net.Sockets;
using System.Text;

namespace MekashronLoginPage.Application;

public interface IAuthorizationProvider
{
    Task<bool> LoginAsync(string userName, string password);
}

public class AuthorizationProvider : IAuthorizationProvider
{
    private readonly IAuthorizationConfigProvider _configProvider;
    private readonly IHttpClientFactory _clientFactory;

    public AuthorizationProvider(
        IAuthorizationConfigProvider configProvider,
        IHttpClientFactory clientFactory)
    {
        _configProvider = configProvider;
        _clientFactory = clientFactory;
    }
    
    public async Task<bool> LoginAsync(string userName, string password)
    {
        // var ip = (await Dns.GetHostEntryAsync(Dns.GetHostName())).AddressList
        //     .FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
        
        var soapEnvelope = $@"
            <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/""
                              xmlns:urn=""urn:ICUTech.Intf-IICUTech"">
               <soapenv:Header/>
               <soapenv:Body>
                  <urn:Login>
                     <urn:UserName>{userName}</urn:UserName>
                     <urn:Password>{password}</urn:Password>
                     <urn:IPs>127.0.0.1</urn:IPs>
                  </urn:Login>
               </soapenv:Body>
            </soapenv:Envelope>";
        
        using var httpClient = _clientFactory.CreateClient();

        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(_configProvider.GetAuthUrl()),
            Method = HttpMethod.Post,
            Content = new StringContent(soapEnvelope, Encoding.UTF8, MediaTypeNames.Text.Xml),
        };
        httpRequest.Headers.Add("SOAPAction", "\"urn:ICUTech.Intf-IICUTech#Login\"");
        httpRequest.Headers.Add("Access-Control-Allow-Origin", "*");

        var responseContent = await httpClient.SendAsync(httpRequest);
            
        return responseContent.IsSuccessStatusCode;
    }
}
