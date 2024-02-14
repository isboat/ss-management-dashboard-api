using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IRepository<ScreenModel> _repository;
        private readonly IRepository<AssetItemModel> _assetRepository;
        private readonly IRepository<MenuModel> _menuRepository;
        private readonly IPlaylistsService _playlistsService;
        private readonly ITextAssetService _textAssetService;

        public PreviewService(
            IRepository<ScreenModel> repository,
            IRepository<AssetItemModel> assetRepository,
            IRepository<MenuModel> menuRepository,
            IPlaylistsService playlistsService,
            ITextAssetService textAssetService)
        {
            _repository = repository;
            _assetRepository = assetRepository;
            _menuRepository = menuRepository;
            _playlistsService = playlistsService;
            _textAssetService = textAssetService;
        }

        public async Task<DetailedScreenModel?> GetDataAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            if (screen == null) return null;

            var screenDetails = DetailedScreenModel.ToDetails(screen);

            if (!string.IsNullOrEmpty(screenDetails.MenuEntityId))
            {
                screenDetails.Menu = await GetMenuDetails(tenantId, screenDetails.MenuEntityId);
            }

            if(!string.IsNullOrEmpty(screenDetails.PlaylistId))
            {
                screenDetails.PlaylistData = await _playlistsService.GetWithItemsAsync(tenantId, screenDetails.PlaylistId);
            }

            if (!string.IsNullOrEmpty(screenDetails.MediaAssetEntityId))
            {
                screenDetails.MediaAsset = await GetMediaAssetDetails(tenantId, screenDetails.MediaAssetEntityId);
            }

            if (!string.IsNullOrEmpty(screenDetails.TextAssetEntityId))
            {
                screenDetails.TextEditorData = await GetTextAssetDetails(tenantId, screenDetails.TextAssetEntityId);
            }

            return screenDetails;
        }

        private async Task<string?> GetTextAssetDetails(string tenantId, string textAssetEntityId)
        {
            var asset = await _textAssetService.GetAsync(tenantId, textAssetEntityId);
            return asset?.Description;
        }

        private async Task<MenuModel?> GetMenuDetails(string tenantId, string itemId)
        {
            var menu = await _menuRepository.GetAsync(tenantId, itemId);
            return menu;
        }

        private async Task<AssetItemModel?> GetMediaAssetDetails(string tenantId, string itemId)
        {
            var asset = await _assetRepository.GetAsync(tenantId, itemId);
            return asset;
        }
    }
}