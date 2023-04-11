namespace Management.Dashboard.Models
{
    public class DisplayItem
    { 
        public string? ItemId { get; set; }

        public DisplayItemType? ItemType { get; set; }
    }

    public enum DisplayItemType
    {
        Menu,
        MediaAsset
    }
}