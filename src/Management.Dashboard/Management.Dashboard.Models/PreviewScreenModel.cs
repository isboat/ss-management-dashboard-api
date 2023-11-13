namespace Management.Dashboard.Models
{
    public class PreviewScreenModel : ScreenModel
    {
        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public static PreviewScreenModel ToDetails(ScreenModel screen)
        {
            var details = new PreviewScreenModel
            {
                DisplayName = screen.DisplayName,
                Id = screen.Id,
                TenantId = screen.TenantId,
                MenuEntityId = screen.MenuEntityId,
                MediaAssetEntityId = screen.MediaAssetEntityId,
                TemplateKey = screen.TemplateKey,
                TemplateProperties = screen.TemplateProperties,
                ExternalMediaSource = screen.ExternalMediaSource
            };

            return details;
        }
    }
}