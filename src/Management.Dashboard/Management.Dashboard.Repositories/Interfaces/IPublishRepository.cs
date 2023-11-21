using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IPublishRepository
    {
        Task<bool> PublishScreenAsync(DetailedScreenModel model);
    }
}
