using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class TextAssetItemModel : IModelItem, IPlaylistItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? BackgroundColor { get; set; }

        public string? TextColor { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public PlaylistItemType PlaylistType => PlaylistItemType.Text;
    }
}
