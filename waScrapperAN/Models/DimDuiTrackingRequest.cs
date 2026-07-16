using System.Text;

namespace waScrapperAN.Models;

public sealed class DimDuiTrackingRequest
{
    public string Gestion { get; init; } = string.Empty;
    public string Aduana { get; init; } = string.Empty;
    public string Numero { get; init; } = string.Empty;
    public string Serie { get; init; } = string.Empty;

    public string ToInternalXml()
    {
        var doc = new System.Xml.Linq.XDocument(
            new System.Xml.Linq.XDeclaration("1.0", "UTF-8", "no"),
            new System.Xml.Linq.XElement("solicitud",
                new System.Xml.Linq.XElement("gestion", Gestion),
                new System.Xml.Linq.XElement("aduana", Aduana),
                new System.Xml.Linq.XElement("serial", Serie.ToUpperInvariant()),
                new System.Xml.Linq.XElement("registro", Numero),
                new System.Xml.Linq.XElement("usuario", "ANDROID")
            )
        );
        return doc.ToString(System.Xml.Linq.SaveOptions.DisableFormatting);
    }
}