using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class MenuRepository : IRepository<MenuModel>
    {
        private readonly MongoClient _client;
        private readonly string CollectionName = "Menus";

        public MenuRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
        }

        private IMongoCollection<MenuModel> GetTenantScreenCollection(string tenantId)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<MenuModel>(CollectionName);
        }

        public async Task<List<MenuModel>> GetAllByTenantIdAsync(string tenantId)
        {
            return await GetTenantScreenCollection(tenantId).Find(x => x.TenantId == tenantId).ToListAsync();
        }

        public async Task<MenuModel?> GetAsync(string tenantId, string id)
        {
            return await GetTenantScreenCollection(tenantId).Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();
        }

        public async Task CreateAsync(MenuModel newModel)
        {
            EnsureIdNotNull(newModel);
            await GetTenantScreenCollection(newModel.TenantId!).InsertOneAsync(newModel);
        }

        public async Task UpdateAsync(string id, MenuModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            await GetTenantScreenCollection(updatedModel.TenantId!).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await GetTenantScreenCollection(tenantId).DeleteOneAsync(x => x.Id == id && x.TenantId == tenantId);
        }

        private static void EnsureIdNotNull(MenuModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }
    }
}