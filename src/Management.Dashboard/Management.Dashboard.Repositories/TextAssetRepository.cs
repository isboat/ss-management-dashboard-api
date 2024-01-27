using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class TextAssetRepository : BaseRepository, IRepository<TextAssetItemModel>
    {
        private readonly string CollectionName = "TextAssetItems";

        public TextAssetRepository(IOptions<MongoSettings> settings): base(settings)
        {
        }

        public async Task<List<TextAssetItemModel>> GetAllByTenantIdAsync(string tenantId)
        {
            return await GetAllByTenantIdAsync<TextAssetItemModel>(tenantId, CollectionName);
        }

        public async Task<TextAssetItemModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<TextAssetItemModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(TextAssetItemModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;
            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, TextAssetItemModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<TextAssetItemModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<TextAssetItemModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(TextAssetItemModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }
    }
}