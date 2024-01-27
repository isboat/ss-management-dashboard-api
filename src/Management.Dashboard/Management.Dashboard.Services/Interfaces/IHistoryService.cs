using Management.Dashboard.Models.History;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IHistoryService
    {
        Task StoreAsync(HistoryModel model);

        Task<IEnumerable<HistoryModel>> GetItemHistoriesAsync(string tenantId, string historyItemId);

        Task<IEnumerable<HistoryModel>> GetAllByTenantIdAsync(string tenantId);

        Task<IEnumerable<HistoryModel>> GetByItemTypeAsync(string tenantId, string historyItemType);
    }
}
