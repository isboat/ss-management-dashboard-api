namespace Management.Dashboard.Models.History
{
    public class HistoryModel : IModelItem
    {
        public string? ItemId { get; set; }

        /// <summary>
        /// Example: ScreenModel, MenuModel. This should be the class name
        /// </summary>
        public string? ItemType { get; set; }

        public string? Log { get; set; }

        public string? User { get; set; }

        public DateTime DateTimeStamp { get; set; }

        public string? Id { get; set; }
        public string? TenantId { get; set; }
    }
}
