using System.Net;
using System.Xml.Linq;
using waScrapperAN.Models;

namespace waScrapperAN.Utilities;

public sealed class DimDuiSoapResponseParser
{
    public DimDuiSoapResponseParser()
    {
    }

    public DimDuiTrackingResult Parse(string soapResponse, DimDuiTrackingRequest request, GeneratedTrackingFiles archivos)
    {
        // Clear unused variable warnings
        _ = archivos;
        return ParseInternal(soapResponse, request, archivos);
    }

    private DimDuiTrackingResult ParseInternal(string soapResponse, DimDuiTrackingRequest request, GeneratedTrackingFiles archivos)
    {
        XDocument soapDoc;
        try
        {
            soapDoc = XDocument.Parse(soapResponse);
        }
        catch (Exception)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "La respuesta recibida no tiene un formato XML válido.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Check for SOAP Fault
        var faultElement = soapDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "Fault");
        if (faultElement is not null)
        {
            var faultCode = faultElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "faultcode")?.Value ?? string.Empty;
            var faultString = faultElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "faultstring")?.Value ?? string.Empty;
            var faultDetail = faultElement.Descendants().FirstOrDefault(e => e.Name.LocalName == "detail")?.Value ?? string.Empty;
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = $"Error del servicio: {faultString}",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Find reporteClickResult or return
        var resultElement = soapDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "reporteClickResult")
                           ?? soapDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "return");

        if (resultElement is null)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "La respuesta SOAP no contiene reporteClickResult ni return.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        var rawText = resultElement.Value;

        // HTML decode the inner text
        var decodedText = WebUtility.HtmlDecode(rawText)?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(decodedText))
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "El servicio respondió vacío.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Check initial code
        if (decodedText.StartsWith('0'))
        {
            var errorMsg = decodedText.Length > 1 ? decodedText[1..].Trim() : "Error desconocido del servicio.";
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = errorMsg,
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        string cleanXml;
        if (decodedText.StartsWith('1'))
        {
            cleanXml = decodedText[1..].Trim();
        }
        else
        {
            cleanXml = decodedText;
        }

        // Find first '<' to locate XML start
        var xmlStart = cleanXml.IndexOf('<');
        if (xmlStart < 0)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "El servicio respondió, pero no devolvió un XML de seguimiento.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        cleanXml = cleanXml[xmlStart..];

        return ParseInternalXml(cleanXml, request, archivos);
    }

    private static DimDuiTrackingResult ParseInternalXml(string xmlContent, DimDuiTrackingRequest request, GeneratedTrackingFiles archivos)
    {
        XDocument internalDoc;
        try
        {
            internalDoc = XDocument.Parse(xmlContent);
        }
        catch (Exception)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "La respuesta recibida no tiene un formato XML válido.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Find click root element
        var clickElement = internalDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "click");
        if (clickElement is null)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "La Aduana respondió, pero no se encontró información de seguimiento.",
                Archivos = archivos,
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Read DIM
        var dimElement = clickElement.Element("dim");
        var dim = dimElement?.Value?.Trim() ?? string.Empty;

        if (string.IsNullOrWhiteSpace(dim))
        {
            dim = $"DI-{request.Gestion}-{request.Aduana}-{request.Serie}-{request.Numero}";
        }

        // Read all estado nodes
        var estadoNodes = clickElement.Elements().Where(e => e.Name.LocalName == "estado").ToList();
        var eventos = new List<DimDuiTrackingEvent>();

        foreach (var estadoNode in estadoNodes)
        {
            var etapa = ReadElementValue(estadoNode, "etapa");
            var fecha = ReadElementValue(estadoNode, "fecha");
            var hora = ReadElementValue(estadoNode, "hora");
            var canal = ReadElementValue(estadoNode, "canal");
            var estado = ReadElementValue(estadoNode, "estado");
            var observacion = ReadElementValue(estadoNode, "observacion");
            if (string.IsNullOrWhiteSpace(observacion))
            {
                observacion = ReadElementValue(estadoNode, "obs");
            }
            var usuario = ReadElementValue(estadoNode, "usuario");

            var fechaHora = $"{fecha} {hora}".Trim();

            eventos.Add(new DimDuiTrackingEvent
            {
                Dim = dim,
                Gestion = request.Gestion,
                Aduana = request.Aduana,
                Serie = request.Serie,
                Numero = request.Numero,
                Etapa = etapa,
                Fecha = fecha,
                Hora = hora,
                FechaHora = fechaHora,
                Canal = canal,
                Estado = estado,
                Observacion = observacion,
                Usuario = usuario
            });
        }

        return new DimDuiTrackingResult
        {
            IsSuccess = true,
            Message = eventos.Count > 0 ? "Consulta completada correctamente." : "No se encontraron eventos.",
            Dim = dim,
            Eventos = eventos.AsReadOnly(),
            Archivos = archivos
        };
    }

    private static string ReadElementValue(XElement parent, string localName)
    {
        return parent.Elements()
            .FirstOrDefault(e => e.Name.LocalName == localName)
            ?.Value?.Trim() ?? string.Empty;
    }
}