﻿using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class AssetItemModel : IModelItem, IPlaylistItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; }

        public string? AssetUrl { get; set; }

        public string? FileName { get; set; }

        public AssetType? Type { get; set; }
        public PlaylistItemType PlaylistType => PlaylistItemType.Media;

        public DateTime? CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }

    public enum AssetType
    {
        None,
        Image,
        Video
    }
}
