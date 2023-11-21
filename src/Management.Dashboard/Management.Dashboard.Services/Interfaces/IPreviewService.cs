using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IPreviewService
    {
        Task<DetailedScreenModel?> GetDataAsync(string tenantId, string id);
    }
}
