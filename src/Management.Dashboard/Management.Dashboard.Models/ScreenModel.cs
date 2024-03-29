﻿using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class ScreenModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? DisplayName { get; set; }

        public string? MenuEntityId { get; set; }

        public string? MediaAssetEntityId { get; set; }

        public string? TextAssetEntityId { get; set; }

        public LayoutModel? Layout { get; set; }

        public string? ExternalMediaSource { get; set; }

        public string? TextEditorData { get; set; } = string.Empty;

        public string? PlaylistId { get; set; }

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    [BsonIgnoreExtraElements]
    public class LayoutModel
    {
        public string? TemplateKey { get; set; }

        public IEnumerable<TemplatePropertyModel>? TemplateProperties { get; set; }

        public string? SubType { get; set; }
    }
}