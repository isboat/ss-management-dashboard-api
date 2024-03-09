using Management.Dashboard.Models;
using Management.Dashboard.Models.History;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class HistoryService : IHistoryService
    {
        private readonly IHistoryRepository _repository;

        public HistoryService(IHistoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<HistoryModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await _repository.GetAllByTenantIdAsync(tenantId, skip, limit);
        }

        public async Task<IEnumerable<HistoryModel>> GetByItemTypeAsync(string tenantId, string historyItemType, int? skip, int? limit)
        {
            return await _repository.GetByItemTypeAsync(tenantId, historyItemType, skip, limit);
        }

        public async Task<IEnumerable<HistoryModel>> GetItemHistoriesAsync(string tenantId, string historyItemId)
        {
            return await _repository.GetItemHistoriesAsync(tenantId, historyItemId);
        }

        public async Task StoreAsync(HistoryModel model)
        {
            model.CreatedOn = DateTime.UtcNow;
            await _repository.CreateAsync(model);
        }
    }
}
