using Management.Dashboard.Models.History;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IHistoryRepository : IRepository<HistoryModel>
    {
        Task<IEnumerable<HistoryModel>> GetItemHistoriesAsync(string tenantId, string historyItemId);

        Task<IEnumerable<HistoryModel>> GetByItemTypeAsync(string tenantId, string historyItemType);
    }
}
