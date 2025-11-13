using System.Xml.Serialization;
using MekashronLoginPage.Application.Utils;

namespace MekashronLoginPage.Application.Models;

[XmlRoot(ElementName = Constants.EnvelopeElement, Namespace = Constants.SoapEnvelopeNamespace)]
public class XmlEnvelope
{
    [XmlElement(ElementName = Constants.BodyElement, Namespace = Constants.SoapEnvelopeNamespace)]
    public XmlBody XmlBody { get; set; } = null!;
}

public class XmlBody
{
    [XmlElement(ElementName = Constants.LoginResponseElement, Namespace = Constants.IcuTechNamespace)]
    public XmlLoginResponse XmlLoginResponse { get; set; } = null!;
}

public class XmlLoginResponse
{
    [XmlElement(ElementName = Constants.ReturnElement, Namespace = Constants.EmptyNamespace)]
    public string Return { get; set; } = null!;
}

