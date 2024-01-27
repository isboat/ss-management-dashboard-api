using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class MenuItem : IModelItem
    {
        public string? Id { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? IconUrl { get; set; }

        public string? Price { get; set; }

        public string? DiscountPrice { get; set; }
        public string? TenantId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
}
