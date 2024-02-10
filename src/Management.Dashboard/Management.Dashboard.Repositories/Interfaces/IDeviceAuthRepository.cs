using Management.Dashboard.Models;

namespace Management.Dashboard.Repositories.Interfaces
{
    public interface IDeviceAuthRepository<T> : IRepository<T>
    {
        Task<T?> GetByDeviceUsercodeAsync(string deviceUsercode);

        Task<List<DeviceAuthModel>> GetByFilterAsync(Func<DeviceAuthModel, bool> filterFunc);

        Task<DeviceAuthApprovalStatus> ApproveAsync(DeviceAuthModel updateModel);
    }
}
