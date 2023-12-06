using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PlaylistsService : IPlaylistsService
    {
        private readonly IRepository<PlaylistModel> _repository;
        private readonly IAssetService _assetService;

        public PlaylistsService(IRepository<PlaylistModel> repository, IAssetService assetService)
        {
            _repository = repository;
            _assetService = assetService;
        }

        public async Task CreateAsync(PlaylistModel newModel)
        {
            AddId(newModel);
            newModel.Created = DateTime.UtcNow;

            await _repository.CreateAsync(newModel);
        }

        public async Task<IEnumerable<PlaylistModel>> GetAllAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<PlaylistModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<PlaylistWithItemModel?> GetWithMediaAsync(string tenantId, string id)
        {
            var playlist = await _repository.GetAsync(tenantId, id);
            if (playlist == null) return null;

            var withItem = new PlaylistWithItemModel(playlist);
            if (playlist?.AssetIds != null && playlist.AssetIds.Any())
            {

                foreach (var assetId in playlist.AssetIds)
                {
                    var media = await _assetService.GetAsync(tenantId, assetId);
                    if (media != null) 
                    {
                        withItem.AssetItems?.Add(media);
                    }                    
                }

                return withItem;
            }

            return withItem;
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, PlaylistModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}