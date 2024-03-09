using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class MenuRepository : BaseRepository, IRepository<MenuModel>
    {
        private readonly string CollectionName = "Menus";

        public MenuRepository(IOptions<MongoSettings> settings):base(settings) { }


        public async Task<List<MenuModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await GetAllByTenantIdAsync<MenuModel>(tenantId, CollectionName, skip, limit);
        }

        public async Task<MenuModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<MenuModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(MenuModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;
            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, MenuModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<MenuModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<MenuModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(MenuModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }

        public async Task<IEnumerable<MenuModel>> GetByFilterAsync(string tenantId, FilterDefinition<MenuModel> filter) =>
            await GetTenantCollection<MenuModel>(tenantId, CollectionName).Find(filter).ToListAsync();
    }
}