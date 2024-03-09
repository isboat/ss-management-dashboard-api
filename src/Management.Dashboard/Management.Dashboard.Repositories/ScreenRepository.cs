using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class ScreenRepository : BaseRepository, IRepository<ScreenModel>
    {
        private readonly string CollectionName = "Screens";

        public ScreenRepository(IOptions<MongoSettings> settings):base(settings)
        {
        }

        public async Task<List<ScreenModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await GetAllByTenantIdAsync<ScreenModel>(tenantId, CollectionName, skip, limit);
        }

        public async Task<IEnumerable<ScreenModel>> GetByFilterAsync(string tenantId, FilterDefinition<ScreenModel> filter)
        {
            return await GetByFilterAsync(tenantId, CollectionName, filter);
        }

        public async Task<ScreenModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<ScreenModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(ScreenModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;

            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, ScreenModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<ScreenModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<ScreenModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(ScreenModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }
    }
}