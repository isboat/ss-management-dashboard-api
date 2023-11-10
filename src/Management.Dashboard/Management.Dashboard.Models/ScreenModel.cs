namespace Management.Dashboard.Models
{
    public class ScreenModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public string? MenuEntityId { get; set; }

        public string? MediaAssetEntityId { get; set; }

        public string? TemplateKey { get; set; }

        public IEnumerable<TemplatePropertyModel>? TemplateProperties { get; set; }

        public string? ExternalMediaSource { get; set; }
    }
}