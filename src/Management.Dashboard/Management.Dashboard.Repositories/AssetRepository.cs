using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class AssetRepository : BaseRepository, IRepository<AssetItemModel>
    {
        private readonly string CollectionName = "AssetItems";

        public AssetRepository(IOptions<MongoSettings> settings):base(settings) { }

        private IMongoCollection<AssetItemModel> GetTenantScreenCollection(string tenantId)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<AssetItemModel>(CollectionName);
        }

        public async Task<List<AssetItemModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await GetAllByTenantIdAsync<AssetItemModel>(tenantId, CollectionName, skip, limit);
        }

        public async Task<AssetItemModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<AssetItemModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(AssetItemModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;
            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, AssetItemModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<AssetItemModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<AssetItemModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(AssetItemModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }
    }
}