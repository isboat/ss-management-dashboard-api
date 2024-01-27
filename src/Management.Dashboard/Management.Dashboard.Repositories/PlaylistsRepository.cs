using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class PlaylistsRepository : BaseRepository, IRepository<PlaylistModel>
    {
        private readonly string CollectionName = "Playlists";

        public PlaylistsRepository(IOptions<MongoSettings> settings):base(settings)
        {
        }

        public async Task<List<PlaylistModel>> GetAllByTenantIdAsync(string tenantId)
        {
            return await GetAllByTenantIdAsync<PlaylistModel>(tenantId, CollectionName);
        }

        public async Task<PlaylistModel?> GetAsync(string tenantId, string id)
        {
            return await GetAsync<PlaylistModel>(tenantId, CollectionName, id);
        }

        public async Task CreateAsync(PlaylistModel newModel)
        {
            EnsureIdNotNull(newModel);
            newModel.CreatedOn = DateTime.UtcNow;
            await CreateAsync(newModel.TenantId!, CollectionName, newModel);
        }

        public async Task UpdateAsync(string id, PlaylistModel updatedModel)
        {
            EnsureIdNotNull(updatedModel);
            updatedModel.UpdatedOn = DateTime.UtcNow;
            await GetTenantCollection<PlaylistModel>(updatedModel.TenantId!, CollectionName).ReplaceOneAsync(x => x.Id == id, updatedModel);
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            if (string.IsNullOrEmpty(id)) throw new ArgumentNullException(nameof(id));
            if (string.IsNullOrEmpty(tenantId)) throw new ArgumentNullException(nameof(tenantId));

            await RemoveAsync<PlaylistModel>(tenantId, CollectionName, id);
        }

        private static void EnsureIdNotNull(PlaylistModel newModel)
        {
            if (newModel == null) throw new ArgumentNullException(nameof(newModel));
            if (string.IsNullOrEmpty(newModel.Id)) throw new ArgumentNullException(nameof(newModel.Id));
            if (string.IsNullOrEmpty(newModel.TenantId)) throw new ArgumentNullException(nameof(newModel.TenantId));
        }
    }
}