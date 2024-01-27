using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class DeviceModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? ScreenId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}