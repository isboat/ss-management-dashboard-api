namespace Management.Dashboard.Models
{
    public class DetailedScreenModel : ScreenModel
    {
        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public static DetailedScreenModel ToDetails(ScreenModel screen)
        {
            var details = new DetailedScreenModel
            {
                DisplayName = screen.DisplayName,
                Id = screen.Id,
                TenantId = screen.TenantId,
                MenuEntityId = screen.MenuEntityId,
                MediaAssetEntityId = screen.MediaAssetEntityId,
                ExternalMediaSource = screen.ExternalMediaSource,
                Layout = screen.Layout
            };

            return details;
        }
    }
}