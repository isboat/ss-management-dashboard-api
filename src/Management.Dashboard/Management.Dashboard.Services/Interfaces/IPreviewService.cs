using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IPreviewService
    {
        Task<ScreenModel?> GetDataAsync(string tenantId, string id);
    }
}
