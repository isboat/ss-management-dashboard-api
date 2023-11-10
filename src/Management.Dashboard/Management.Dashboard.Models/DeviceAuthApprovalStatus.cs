namespace Management.Dashboard.Models
{
    public enum DeviceAuthApprovalStatus
    {
        Success,
        Failed,
        NotFound,
        BadRequest,
        TenantNotFound,
        DeviceLimitReached,
        AlreadyApproved
    }
}