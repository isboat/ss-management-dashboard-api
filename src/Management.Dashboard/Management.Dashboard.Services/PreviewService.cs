using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class PreviewService : IPreviewService
    {
        private readonly IRepository<ScreenModel> _repository;

        public PreviewService(IRepository<ScreenModel> repository)
        {
            _repository = repository;
        }

        public async Task<PreviewScreenModel?> GetDataAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            if (screen == null) return null;

            var screenDetails = PreviewScreenModel.ToDetails(screen);

            if (!string.IsNullOrEmpty(screenDetails.MenuEntityId))
            {
                screenDetails.Menu = GetMenuDetails(screenDetails.MenuEntityId);
            }

            if (!string.IsNullOrEmpty(screenDetails.MediaAssetEntityId))
            {
                screenDetails.MediaAsset = GetMediaAssetDetails(screenDetails.MediaAssetEntityId);
            }

            return screenDetails;
        }

        private MenuModel? GetMenuDetails(string? itemId)
        {
            return null;
            //throw new NotImplementedException();
        }

        private AssetItemModel? GetMediaAssetDetails(string? itemId)
        {
            return null;
            //throw new NotImplementedException();
        }
    }
}