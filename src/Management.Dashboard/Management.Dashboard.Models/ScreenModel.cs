namespace Management.Dashboard.Models
{
    public class ScreenModel
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public DisplayItem? DisplayItem { get; set; }

        public string? TemplateKey { get; set; }

        public IEnumerable<TemplatePropertyModel>? TemplateProperties { get; set; }
    }

    public class LayoutTemplate
    {
        // Just menu on screen templates: BEGIN
        public const string MenuBasic = "MenuBasic";

        // Just menu on screen templates: END

        public const string MenuOverlay = "MenuOverlay";
        public const string A2 = "A2";
        public const string A3 = "A3";
    }

    public class TemplatePropertyModel
    {
        public string? Key { get; set; }
                     
        public string? Value { get; set; }
                     
        public string? Label { get; set; }
    }

    public class DisplayItem
    { 
        public string ItemId { get; set; } = string.Empty;

        public DisplayItemType? ItemType { get; set; }
    }

    public enum DisplayItemType
    {
        None,
        Menu,
        MediaAsset
    }
}