using Management.Dashboard.Models.History;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class HistoryRepository : BaseRepository, IHistoryRepository
    {
        private readonly string CollectionName = "Histories";

        public HistoryRepository(IOptions<MongoSettings> settings):base(settings)
        {
        }

        public async Task<List<HistoryModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await GetAllByTenantIdAsync<HistoryModel>(tenantId, CollectionName, skip, limit);
        }

        public async Task<HistoryModel?> GetAsync(string tenantId, string id) =>
            await GetAsync<HistoryModel>(tenantId, CollectionName, id);


        public async Task CreateAsync(HistoryModel model)
        {
            model.Id = Guid.NewGuid().ToString("N");
            await CreateAsync(model.TenantId!, CollectionName, model);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            await Task.CompletedTask;
            //await RemoveAsync<HistoryModel>(tenantId, CollectionName, id);
        }

        public async Task<IEnumerable<HistoryModel>> GetItemHistoriesAsync(string tenantId, string historyItemId)
        {
            var collection = GetTenantCollection<HistoryModel>(tenantId, CollectionName);
            return await collection.Find(x => x.TenantId == tenantId && x.ItemId == historyItemId).SortByDescending(x => x.CreatedOn).ToListAsync();
        }

        public async Task<IEnumerable<HistoryModel>> GetByItemTypeAsync(string tenantId, string historyItemType, int? skip, int? limit)
        {
            var collection = GetTenantCollection<HistoryModel>(tenantId, CollectionName);
            return await collection.Find(x => x.TenantId == tenantId && x.ItemType == historyItemType).Skip(skip).Limit(limit).ToListAsync();
        }

        public Task UpdateAsync(string id, HistoryModel updatedModel)
        {
            return Task.CompletedTask;
        }
    }
}