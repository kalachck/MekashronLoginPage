using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Xml.Serialization;
using MekashronLoginPage.Application.Dtos;
using MekashronLoginPage.Application.Models;
using MekashronLoginPage.Application.Utils;

namespace MekashronLoginPage.Application.Providers;

public interface ILoginProvider
{
    Task<LoginResponseDto?> LoginAsync(string userName, string password);
}

public class LoginProvider : ILoginProvider
{
    private readonly ILoginConfigProvider _configProvider;
    private readonly HttpClient _httpClient;

    public LoginProvider(
        ILoginConfigProvider configProvider,
        HttpClient httpClient)
    {
        _configProvider = configProvider;
        _httpClient = httpClient;
    }
    
    public async Task<LoginResponseDto?> LoginAsync(string userName, string password)
    {
        var soapEnvelope = BuildSoapEnvelope(userName, password);

        var httpRequest = CreateHttpRequest(soapEnvelope);
        var response = await _httpClient.SendAsync(httpRequest);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        var envelope = await DeserializeEnvelopeAsync(response);
        if (envelope is null)
        {
            return null;
        }

        var loginResponse = DeserializeLoginResponse(envelope);
        return ValidateLogin(loginResponse, userName);
    }

    private static string BuildSoapEnvelope(string userName, string password) 
        => $@"
            <{Constants.SoapEnvelopeElement} xmlns:soapenv=""{Constants.SoapEnvelopeNamespace}""
                              xmlns:urn=""{Constants.IcuTechNamespace}"">
               <{Constants.SoapHeaderElement}/>
               <{Constants.SoapBodyElement}>
                  <{Constants.LoginElement}>
                     <{Constants.UserNameElement}>{userName}</{Constants.UserNameElement}>
                     <{Constants.PasswordElement}>{password}</{Constants.PasswordElement}>
                     <{Constants.IPsElement}>{Constants.DefaultLocalhostIP}</{Constants.IPsElement}>
                  </{Constants.LoginElement}>
               </{Constants.SoapBodyElement}>
            </{Constants.SoapEnvelopeElement}>";

    private HttpRequestMessage CreateHttpRequest(string soapEnvelope)
    {
        var httpRequest = new HttpRequestMessage
        {
            RequestUri = new Uri(_configProvider.GetSoapUrl()),
            Method = HttpMethod.Post,
            Content = new StringContent(soapEnvelope, Encoding.UTF8, MediaTypeNames.Text.Xml),
        };

        httpRequest.Headers.Add(Constants.SoapActionHeader, Constants.SoapActionLogin);
        httpRequest.Headers.Add(Constants.AccessControlAllowOriginHeader, Constants.AccessControlAllowOriginValue);

        return httpRequest;
    }

    private static async Task<XmlEnvelope?> DeserializeEnvelopeAsync(HttpResponseMessage response)
    {
        var responseString = await response.Content.ReadAsStringAsync();
        if (string.IsNullOrWhiteSpace(responseString))
        {
            return null;
        }

        var xmlSerializer = new XmlSerializer(typeof(XmlEnvelope));
        using var reader = new StringReader(responseString);

        return xmlSerializer.Deserialize(reader) as XmlEnvelope;
    }

    private static LoginResponseDto? DeserializeLoginResponse(XmlEnvelope xmlEnvelope)
    {
        var jsonResponse = xmlEnvelope.XmlBody?.XmlLoginResponse?.Return;
        
        return string.IsNullOrWhiteSpace(jsonResponse) ? null : JsonSerializer.Deserialize<LoginResponseDto>(jsonResponse);
    }

    private static LoginResponseDto? ValidateLogin(LoginResponseDto? loginResponse, string userName)
    {
        if (loginResponse is null)
        {
            return null;
        }

        return string.Equals(loginResponse.Email, userName, StringComparison.OrdinalIgnoreCase)
            ? loginResponse
            : null;
    }
}
