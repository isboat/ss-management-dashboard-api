using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class MenuModel: IModelItem
    {
        public string? Id { get; set; }

        public string? TenantId { get; set; }

        public string? Name { get; set; } = null;

        public string? Description { get; set; } 

        public string? Title { get; set; }

        public string? Currency { get; set; }

        public string? IconUrl { get; set; }

        public IEnumerable<MenuItem>? MenuItems { get; set; }
    }
}
