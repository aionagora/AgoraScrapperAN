using waScrapperAN.Models;

namespace waScrapperAN.Services;

public interface IDimDuiEvidenceWriter
{
    Task<GeneratedTrackingFiles> WriteEvidencesAsync(
        DimDuiTrackingRequest request,
        string solicitudSoap,
        string respuestaSoap,
        string? resultadoInterno,
        string? errorSoap,
        IReadOnlyList<DimDuiTrackingEvent> eventos,
        CancellationToken cancellationToken = default);
}