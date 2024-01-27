using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PlaylistsService : IPlaylistsService
    {
        private readonly IRepository<PlaylistModel> _repository;
        private readonly IAssetService _assetService;
        private readonly ITextAssetService _textAssetService;

        public PlaylistsService(
            IRepository<PlaylistModel> repository, 
            IAssetService assetService, 
            ITextAssetService textAssetService)
        {
            _repository = repository;
            _assetService = assetService;
            _textAssetService = textAssetService;
        }

        public async Task CreateAsync(PlaylistModel newModel)
        {
            AddId(newModel);

            await _repository.CreateAsync(newModel);
        }

        public async Task<IEnumerable<PlaylistModel>> GetAllAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<PlaylistModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<PlaylistWithItemModel?> GetWithItemsAsync(string tenantId, string id)
        {
            var playlist = await _repository.GetAsync(tenantId, id);
            if (playlist == null) return null;

            var withItem = new PlaylistWithItemModel(playlist);
            if (playlist?.ItemIdAndTypePairs != null && playlist.ItemIdAndTypePairs.Any())
            {
                foreach (var assetPair in playlist.ItemIdAndTypePairs)
                {
                    switch (assetPair.ItemType)
                    {
                        case PlaylistItemType.Media:
                            var media = await _assetService.GetAsync(tenantId, assetPair.Id!);
                            if (media != null) withItem.Items?.Add(media);
                            break;

                        case PlaylistItemType.Text:
                            var textAsset = await _textAssetService.GetAsync(tenantId, assetPair.Id!);
                            if(textAsset != null) withItem.Items?.Add(textAsset);
                            break;

                        default:
                            break;
                    }

                }
            }

            return withItem;
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, PlaylistModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task AddToPlaylist(string tenantId, string id, string itemId, PlaylistItemType itemType)
        {
            var playlist = await _repository.GetAsync(tenantId, id);
            if (playlist == null) return;

            playlist.ItemIdAndTypePairs ??= new List<PlaylistItemIdTypePair>();
            playlist.ItemIdAndTypePairs.Add(new PlaylistItemIdTypePair { Id = itemId, ItemType = itemType });

            await this.UpdateAsync(id, playlist);
        }

        public async Task RemoveFromPlaylist(string tenantId, string id, string assetId)
        {
            var playlist = await _repository.GetAsync(tenantId, id);
            if (playlist?.ItemIdAndTypePairs == null) return;

            var pair = playlist.ItemIdAndTypePairs.FirstOrDefault(x => x.Id == assetId);
            if (pair == null) return;

            playlist.ItemIdAndTypePairs.Remove(pair);

            await this.UpdateAsync(id, playlist);
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}