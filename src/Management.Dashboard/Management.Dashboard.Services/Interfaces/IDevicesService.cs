using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IDevicesService
    {
        Task<IEnumerable<DeviceModel>> GetDevicesAsync(string tenantId, int? skip, int? limit);

        Task<DeviceModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(DeviceModel newModel);

        public Task UpdateAsync(string id, DeviceModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
