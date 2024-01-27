namespace Management.Dashboard.Services.Interfaces
{
    public interface IPublishService
    {
        Task<bool> PublishScreenAsync(string tenantId, string id, string user);

        Task<bool> ArchiveDataAsync(string tenantId, string id, string user);
    }
}
