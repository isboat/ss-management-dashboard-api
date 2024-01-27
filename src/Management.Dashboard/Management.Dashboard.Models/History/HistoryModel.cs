using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models.History
{
    [BsonIgnoreExtraElements]
    public class HistoryModel : IModelItem
    {
        public string? ItemId { get; set; }

        /// <summary>
        /// Example: ScreenModel, MenuModel. This should be the class name
        /// </summary>
        public string? ItemType { get; set; }

        public string? Log { get; set; }

        public string? User { get; set; }

        public string? Id { get; set; }
        public string? TenantId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
