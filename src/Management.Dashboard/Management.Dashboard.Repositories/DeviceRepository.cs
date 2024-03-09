using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class DeviceRepository : BaseRepository, IRepository<DeviceModel>
    {
        private readonly string CollectionName = "Devices";

        public DeviceRepository(IOptions<MongoSettings> settings):base(settings) { }

        public async Task<List<DeviceModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await GetAllByTenantIdAsync<DeviceModel>(tenantId, CollectionName, skip, limit);
        }

        public async Task<DeviceModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<DeviceModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(DeviceModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;
            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, DeviceModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<DeviceModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<DeviceModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(DeviceModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }

        public async Task<IEnumerable<DeviceModel>> GetByFilterAsync(string tenantId, FilterDefinition<DeviceModel> filter) =>
            await GetTenantCollection<DeviceModel>(tenantId, CollectionName).Find(filter).ToListAsync();
    }
}