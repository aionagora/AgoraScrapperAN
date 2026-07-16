using System.Net;
using System.Text;
using System.Xml.Linq;
using waScrapperAN.Configuration;
using waScrapperAN.Models;
using waScrapperAN.Utilities;
using Microsoft.Extensions.Options;

namespace waScrapperAN.Services;

public sealed class DimDuiTrackingService : IDimDuiTrackingService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AduanaNacionalOptions _options;
    private readonly IDimDuiEvidenceWriter _evidenceWriter;
    private readonly DimDuiSoapResponseParser _parser;

    public DimDuiTrackingService(
        IHttpClientFactory httpClientFactory,
        IOptions<AduanaNacionalOptions> options,
        IDimDuiEvidenceWriter evidenceWriter)
    {
        _httpClientFactory = httpClientFactory;
        _options = options.Value;
        _evidenceWriter = evidenceWriter;
        _parser = new DimDuiSoapResponseParser();
    }

    public async Task<DimDuiTrackingResult> ConsultarAsync(
        DimDuiTrackingRequest request,
        CancellationToken cancellationToken = default)
    {
        // Step 1: Build internal XML
        var internalXml = request.ToInternalXml();

        // Step 2: Build SOAP envelope
        var soapEnvelope = BuildSoapEnvelope(internalXml);
        var solicitudSoap = soapEnvelope.ToString();
        System.Diagnostics.Debug.WriteLine(solicitudSoap);

        // Step 3: Send HTTP request
        var httpClient = _httpClientFactory.CreateClient("AduanaNacional");
        using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        cts.CancelAfter(TimeSpan.FromSeconds(_options.TimeoutSeconds));

        string respuestaSoap;
        try
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, _options.Endpoint)
            {
                Content = new StringContent(solicitudSoap, Encoding.UTF8, "text/xml")
            };
            requestMessage.Headers.Add("Accept", "*/*");
            requestMessage.Headers.Add("User-Agent", "ksoap2-android/2.6.0+");
            requestMessage.Headers.Add("SOAPAction", $"\"{_options.Namespace}{_options.MetodoSeguimiento}\"");

            var response = await httpClient.SendAsync(
                requestMessage,
                HttpCompletionOption.ResponseContentRead,
                cts.Token);

            respuestaSoap = await response.Content.ReadAsStringAsync(cts.Token);
            System.Diagnostics.Debug.WriteLine(respuestaSoap);

            if (!response.IsSuccessStatusCode)
            {
                var errorFiles = await _evidenceWriter.WriteEvidencesAsync(
                    request, solicitudSoap, respuestaSoap, null, respuestaSoap,
                    Array.Empty<DimDuiTrackingEvent>(), cancellationToken);

                return new DimDuiTrackingResult
                {
                    IsSuccess = false,
                    Message = $"El servicio respondió con código HTTP {(int)response.StatusCode}.",
                    Archivos = errorFiles,
                    Eventos = Array.Empty<DimDuiTrackingEvent>()
                };
            }
        }
        catch (OperationCanceledException)
        {
            if (cancellationToken.IsCancellationRequested)
                throw; // user cancellation

            // Timeout
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "La consulta excedió el tiempo máximo permitido.",
                Archivos = new GeneratedTrackingFiles(),
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }
        catch (HttpRequestException)
        {
            return new DimDuiTrackingResult
            {
                IsSuccess = false,
                Message = "No fue posible conectar con el servicio de la Aduana Nacional.",
                Archivos = new GeneratedTrackingFiles(),
                Eventos = Array.Empty<DimDuiTrackingEvent>()
            };
        }

        // Step 4: Parse and extract the internal XML from SOAP response
        // First try to extract the inner content from the SOAP response
        var resultadoInterno = ExtractInternalXmlFromSoap(respuestaSoap);

        // Step 5: Parse the internal XML
        var parsedResult = _parser.Parse(respuestaSoap, request, new GeneratedTrackingFiles());

        // Step 6: Save evidences
        try
        {
            var archivos = await _evidenceWriter.WriteEvidencesAsync(
                request,
                solicitudSoap,
                respuestaSoap,
                resultadoInterno,
                !parsedResult.IsSuccess ? respuestaSoap : null,
                parsedResult.Eventos,
                cancellationToken);

            return new DimDuiTrackingResult
            {
                IsSuccess = parsedResult.IsSuccess,
                Message = parsedResult.Message,
                Dim = parsedResult.Dim,
                Eventos = parsedResult.Eventos,
                Archivos = archivos
            };
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error al guardar evidencias: {ex}");

            return new DimDuiTrackingResult
            {
                IsSuccess = parsedResult.IsSuccess,
                Message = parsedResult.IsSuccess
                    ? "La consulta fue recibida, pero ocurrió un error al guardar los archivos de evidencia."
                    : parsedResult.Message,
                Dim = parsedResult.Dim,
                Eventos = parsedResult.Eventos,
                Archivos = parsedResult.Archivos
            };
        }
    }

    private static XDocument BuildSoapEnvelope(string internalXml)
    {
        XNamespace v = "http://schemas.xmlsoap.org/soap/envelope/";
        XNamespace c = "http://schemas.xmlsoap.org/soap/encoding/";
        XNamespace d = "http://www.w3.org/2001/XMLSchema";
        XNamespace i = "http://www.w3.org/2001/XMLSchema-instance";
        XNamespace n0 = "http://wsgerencialanb.aduana.gob.bo/";

        var doc = new XDocument(
            new XDeclaration("1.0", "UTF-8", null),
            new XElement(v + "Envelope",
                new XAttribute(XNamespace.Xmlns + "i", i.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "d", d.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "c", c.NamespaceName),
                new XAttribute(XNamespace.Xmlns + "v", v.NamespaceName),
                new XElement(v + "Header"),
                new XElement(v + "Body",
                    new XElement(n0 + "reporteClick",
                        new XAttribute("id", "o0"),
                        new XAttribute(c + "root", 1),
                        new XAttribute(XNamespace.Xmlns + "n0", n0.NamespaceName),
                        new XElement("inXml",
                            new XAttribute(i + "type", "d:string"),
                            new XText(internalXml)
                        )
                    )
                )
            )
        );

        return doc;
    }

    private static string? ExtractInternalXmlFromSoap(string soapResponse)
    {
        try
        {
            var soapDoc = XDocument.Parse(soapResponse);

            var resultElement = soapDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "reporteClickResult")
                               ?? soapDoc.Descendants().FirstOrDefault(e => e.Name.LocalName == "return");

            if (resultElement is null)
                return null;

            var rawText = resultElement.Value;
            var decodedText = WebUtility.HtmlDecode(rawText)?.Trim() ?? string.Empty;

            if (string.IsNullOrWhiteSpace(decodedText))
                return null;

            if (decodedText.StartsWith('0') || decodedText.StartsWith('1'))
            {
                decodedText = decodedText[1..].Trim();
            }

            var xmlStart = decodedText.IndexOf('<');
            if (xmlStart < 0)
                return null;

            return decodedText[xmlStart..];
        }
        catch (Exception)
        {
            return null;
        }
    }
}