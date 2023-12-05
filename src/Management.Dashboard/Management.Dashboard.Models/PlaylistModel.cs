using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class PlaylistModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; }

        public DateTime? Created { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public IList<string>? AssetIds { get; set; }
    }

    public class PlaylistWithItemModel : PlaylistModel
    {
        public PlaylistWithItemModel(PlaylistModel model)
        {
            this.Id = model.Id;
            this.TenantId = model.TenantId;
            this.Name = model.Name;
            this.Created = model.Created;
            this.ModifiedDate = model.ModifiedDate;
            this.AssetIds = model.AssetIds;
            this.AssetItems = new List<AssetItemModel>();
        }
        public IList<AssetItemModel>? AssetItems { get; set; }
    }
}