using System.Text.Json.Serialization;

namespace Management.Dashboard.Models
{
    [Serializable]
    public class DetailedScreenModel : ScreenModel
    {
        public MenuModel? Menu { get; set; }
        public AssetItemModel? MediaAsset { get; set; }

        public string? Checksum { get; set; }

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
                TextEditorData = screen.TextEditorData,
                Layout = screen.Layout
            };

            return details;
        }
    }
}