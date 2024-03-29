﻿using MongoDB.Bson.Serialization.Attributes;

namespace Management.Dashboard.Models
{
    [BsonIgnoreExtraElements]
    public class DeviceAuthModel
    {
        public DateTime? RegisteredDatetime { get; set; }

        public DateTime? ApprovedDatetime { get; set; }

        public string? TenantId { get; set; }
        public string? Id { get; set; }
        public string? DeviceCode { get; set; }

        public string? DeviceName { get; set; }
        public string? UserCode { get; set; }
        public int? ExpiresIn { get; set; }
        public int? Interval { get; set; }

        public string? ScreenId { get; set; }
    }
    public class DeviceUpdateRequestModel
    {
        public string? Id { get; set; }

        public string? DeviceName { get; set; }

        public string? ScreenId { get; set; }
    }
}