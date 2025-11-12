using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

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
        
        var responseString = await responseContent.Content.ReadAsStringAsync();

        var xmlSerializer = new XmlSerializer(typeof(Envelope));
        using var reader = new StringReader(responseString);
        var envelope = (Envelope)xmlSerializer.Deserialize(reader);

        if (envelope == null)
        {
            return false;
        }
        
        var jsonResponse = envelope.Body.LoginResponse.Return;
        var loginResponse = JsonSerializer.Deserialize<LoginResponseData>(jsonResponse);
        
        return string.Equals(loginResponse?.Email, userName, StringComparison.CurrentCultureIgnoreCase);
    }

    [XmlRoot(ElementName = "Envelope", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
    public class Envelope
    {
        [XmlElement(ElementName = "Body", Namespace = "http://schemas.xmlsoap.org/soap/envelope/")]
        public Body Body { get; set; } = null!;
    }

    public class Body
    {
        [XmlElement(ElementName = "LoginResponse", Namespace = "urn:ICUTech.Intf-IICUTech")]
        public LoginResponse LoginResponse { get; set; } = null!;
    }

    public class LoginResponse
    {
        [XmlElement(ElementName = "return", Namespace = "")]
        public string Return { get; set; } = null!;
    }

    public class LoginResponseData
    {
        public int EntityId { get; init; }
        public string? FirstName { get; init; }
        public string? LastName { get; init; }
        public string? Company { get; init; }
        public string? Address { get; init; }
        public string? City { get; init; }
        public string? Country { get; init; }
        public string? Zip { get; init; }
        public string? Phone { get; init; }
        public string? Mobile { get; init; }
        public string? Email { get; init; }
        public int EmailConfirm { get; init; }
        public int MobileConfirm { get; init; }
        public int CountryID { get; init; }
        public int Status { get; init; }
        [JsonPropertyName("lid")]
        public string? Lid { get; init; }
        public string? FTPHost { get; init; }
        public int FTPPort { get; init; }
    }
}
