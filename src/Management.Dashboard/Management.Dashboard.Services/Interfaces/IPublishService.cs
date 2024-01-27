namespace Management.Dashboard.Services.Interfaces
{
    public interface IPublishService
    {
        Task<bool> PublishScreenAsync(string tenantId, string id);

        Task<bool> ArchiveDataAsync(string tenantId, string id);
    }
}
