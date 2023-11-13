using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IPreviewService
    {
        Task<PreviewScreenModel?> GetDataAsync(string tenantId, string id);
    }
}
