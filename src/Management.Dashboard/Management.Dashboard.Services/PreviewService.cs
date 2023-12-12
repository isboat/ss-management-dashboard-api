using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IRepository<ScreenModel> _repository;
        private readonly IRepository<AssetItemModel> _mediaRepository;
        private readonly IRepository<MenuModel> _menuRepository;
        private readonly IPlaylistsService _playlistsService;

        public PreviewService(
            IRepository<ScreenModel> repository,
            IRepository<AssetItemModel> mediaRepository,
            IRepository<MenuModel> menuRepository,
            IPlaylistsService playlistsService)
        {
            _repository = repository;
            _mediaRepository = mediaRepository;
            _menuRepository = menuRepository;
            _playlistsService = playlistsService;
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
                screenDetails.PlaylistData = await _playlistsService.GetWithMediaAsync(tenantId, screenDetails.PlaylistId);
            }

            if (!string.IsNullOrEmpty(screenDetails.MediaAssetEntityId))
            {
                screenDetails.MediaAsset = await GetMediaAssetDetails(tenantId, screenDetails.MediaAssetEntityId);
            }

            return screenDetails;
        }

        private async Task<MenuModel?> GetMenuDetails(string tenantId, string itemId)
        {
            var menu = await _menuRepository.GetAsync(tenantId, itemId);
            return menu;
        }

        private async Task<AssetItemModel?> GetMediaAssetDetails(string tenantId, string itemId)
        {
            var asset = await _mediaRepository.GetAsync(tenantId, itemId);
            return asset;
        }
    }
}