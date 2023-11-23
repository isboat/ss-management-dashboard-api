using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IPublishRepository
    {
        Task<bool> PublishScreenAsync(DetailedScreenModel model);
        Task<bool> ArchiveScreenAsync(string tenantId, string id);
    }
}
