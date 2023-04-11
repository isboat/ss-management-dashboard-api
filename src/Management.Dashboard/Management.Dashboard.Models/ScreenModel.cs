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
}