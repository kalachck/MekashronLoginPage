namespace MekashronLoginPage.Application.Utils;

public static class Constants
{
    // SOAP/XML Namespaces
    public const string SoapEnvelopeNamespace = "http://schemas.xmlsoap.org/soap/envelope/";
    public const string IcuTechNamespace = "urn:ICUTech.Intf-IICUTech";
    
    // SOAP XML Element Names (with prefixes for XML decomposition)
    public const string SoapEnvelopeElement = "soapenv:Envelope";
    public const string SoapHeaderElement = "soapenv:Header";
    public const string SoapBodyElement = "soapenv:Body";
    public const string LoginElement = "urn:Login";
    public const string UserNameElement = "urn:UserName";
    public const string PasswordElement = "urn:Password";
    public const string IPsElement = "urn:IPs";
    
    // XML Element Names (for attributes)
    public const string EnvelopeElement = "Envelope";
    public const string BodyElement = "Body";
    public const string LoginResponseElement = "LoginResponse";
    public const string ReturnElement = "return";
    public const string EmptyNamespace = "";
    
    // SOAP Action
    public const string SoapActionHeader = "SOAPAction";
    public const string SoapActionLogin = "\"urn:ICUTech.Intf-IICUTech#Login\"";
    
    // HTTP Headers
    public const string AccessControlAllowOriginHeader = "Access-Control-Allow-Origin";
    public const string AccessControlAllowOriginValue = "*";
    
    // Network
    public const string DefaultLocalhostIP = "127.0.0.1";
    
    // JSON Property Names
    public const string JsonPropertyLid = "lid";
}
