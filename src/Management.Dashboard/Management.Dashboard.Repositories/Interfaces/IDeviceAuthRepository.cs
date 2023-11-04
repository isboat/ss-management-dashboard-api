using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IDeviceAuthRepository<T> : IRepository<T>
    {
        Task<T?> GetByDeviceUsercodeAsync(string deviceUsercode);

        Task ApproveAsync(DeviceAuthModel updateModel);
    }
}
