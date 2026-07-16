using waScrapperAN.Models;

namespace waScrapperAN.Services;

public interface IDimDuiTrackingService
{
    Task<DimDuiTrackingResult> ConsultarAsync(
        DimDuiTrackingRequest request,
        CancellationToken cancellationToken = default);
}