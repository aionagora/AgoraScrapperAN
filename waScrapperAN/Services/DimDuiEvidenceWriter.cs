using System.Text;
using System.Text.Json;
using waScrapperAN.Configuration;
using waScrapperAN.Models;
using waScrapperAN.Utilities;
using Microsoft.Extensions.Options;

namespace waScrapperAN.Services;

public sealed class DimDuiEvidenceWriter : IDimDuiEvidenceWriter
{
    private readonly AduanaNacionalOptions _options;

    public DimDuiEvidenceWriter(IOptions<AduanaNacionalOptions> options)
    {
        _options = options.Value;
    }

    public async Task<GeneratedTrackingFiles> WriteEvidencesAsync(
        DimDuiTrackingRequest request,
        string solicitudSoap,
        string respuestaSoap,
        string? resultadoInterno,
        string? errorSoap,
        IReadOnlyList<DimDuiTrackingEvent> eventos,
        CancellationToken cancellationToken = default)
    {
        var outputDir = _options.OutputDirectory;
        Directory.CreateDirectory(outputDir);

        var files = new GeneratedTrackingFiles();

        // 1. Write solicitud-soap.xml
        var solicitudPath = Path.Combine(outputDir, "solicitud-soap.xml");
        await File.WriteAllTextAsync(solicitudPath, solicitudSoap, Encoding.UTF8, cancellationToken);
        files.SolicitudSoapPath = solicitudPath;

        // 2. Write respuesta-soap.xml
        var respuestaPath = Path.Combine(outputDir, "respuesta-soap.xml");
        await File.WriteAllTextAsync(respuestaPath, respuestaSoap, Encoding.UTF8, cancellationToken);
        files.RespuestaSoapPath = respuestaPath;

        // 3. Write resultado-interno.xml
        if (!string.IsNullOrWhiteSpace(resultadoInterno))
        {
            var resultadoPath = Path.Combine(outputDir, "resultado-interno.xml");
            await File.WriteAllTextAsync(resultadoPath, resultadoInterno, Encoding.UTF8, cancellationToken);
            files.ResultadoInternoPath = resultadoPath;
        }

        // 4. Write error-soap.xml if applicable
        if (!string.IsNullOrWhiteSpace(errorSoap))
        {
            var errorPath = Path.Combine(outputDir, "error-soap.xml");
            await File.WriteAllTextAsync(errorPath, errorSoap, Encoding.UTF8, cancellationToken);
            files.ErrorSoapPath = errorPath;
        }

        // 5. Write CSV
        if (eventos.Count > 0)
        {
            var csvContent = CsvUtility.GenerateCsv(eventos);
            var csvPath = Path.Combine(outputDir, "seguimiento-dim-dui.csv");
            await File.WriteAllTextAsync(csvPath, csvContent, Encoding.UTF8, cancellationToken);
            files.CsvPath = csvPath;

            // 6. Write JSON
            var jsonContent = GenerateJson(request, eventos);
            var jsonPath = Path.Combine(outputDir, "seguimiento-dim-dui.json");
            await File.WriteAllTextAsync(jsonPath, jsonContent, Encoding.UTF8, cancellationToken);
            files.JsonPath = jsonPath;
        }

        return files;
    }

    private static string GenerateJson(DimDuiTrackingRequest request, IReadOnlyCollection<DimDuiTrackingEvent> eventos)
    {
        var jsonObj = new
        {
            DIM = eventos.FirstOrDefault()?.Dim ?? string.Empty,
            Parametros = new
            {
                Gestion = request.Gestion,
                Aduana = request.Aduana,
                Numero = request.Numero,
                Serie = request.Serie
            },
            CantidadEventos = eventos.Count,
            Eventos = eventos.Select(e => new
            {
                e.Dim,
                e.Gestion,
                e.Aduana,
                e.Serie,
                e.Numero,
                e.Etapa,
                e.Fecha,
                e.Hora,
                e.FechaHora,
                e.Canal,
                e.Estado,
                Observacion = e.Observacion,
                e.Usuario
            })
        };

        var options = new JsonSerializerOptions
        {
            WriteIndented = true,
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        return JsonSerializer.Serialize(jsonObj, options);
    }
}