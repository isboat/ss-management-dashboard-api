namespace Management.Dashboard.Models
{
    public class DeviceModel : IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? ScreenId { get; set; }
    }
}