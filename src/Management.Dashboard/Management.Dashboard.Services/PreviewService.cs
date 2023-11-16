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

        public PreviewService(
            IRepository<ScreenModel> repository, 
            IRepository<AssetItemModel> mediaRepository, 
            IRepository<MenuModel> menuRepository)
        {
            _repository = repository;
            _mediaRepository = mediaRepository;
            _menuRepository = menuRepository;
        }

        public async Task<PreviewScreenModel?> GetDataAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            if (screen == null) return null;

            var screenDetails = PreviewScreenModel.ToDetails(screen);

            if (!string.IsNullOrEmpty(screenDetails.MenuEntityId))
            {
                screenDetails.Menu = await GetMenuDetails(tenantId, screenDetails.MenuEntityId);
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