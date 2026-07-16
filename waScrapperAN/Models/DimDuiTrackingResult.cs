namespace waScrapperAN.Models;

public sealed class DimDuiTrackingResult
{
    public bool IsSuccess { get; init; }
    public string Message { get; init; } = string.Empty;
    public string Dim { get; init; } = string.Empty;
    public IReadOnlyList<DimDuiTrackingEvent> Eventos { get; init; }
        = Array.Empty<DimDuiTrackingEvent>();
    public GeneratedTrackingFiles Archivos { get; init; }
        = new();
}