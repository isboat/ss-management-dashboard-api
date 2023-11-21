namespace Management.Dashboard.Services.Interfaces
{
    public interface IPublishService
    {
        Task<bool> PublishDataAsync(string tenantId, string id);
    }
}
