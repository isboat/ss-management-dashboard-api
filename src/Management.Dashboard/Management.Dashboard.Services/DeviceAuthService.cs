using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class DeviceAuthService : IDeviceAuthService
    {
        private readonly IDeviceAuthRepository<DeviceAuthModel> _repository;

        public DeviceAuthService(IDeviceAuthRepository<DeviceAuthModel> repository)
        {
            _repository = repository;
        }
        public async Task<bool> ApproveAsync(DeviceAuthModel updatedModel) =>
            await _repository.ApproveAsync(updatedModel);

        public async Task CreateAsync(DeviceAuthModel newModel)
        {
            await _repository.CreateAsync(newModel);
        }

        public async Task<DeviceAuthModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<IEnumerable<DeviceAuthModel>> GetDevicesAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task UpdateAsync(string id, DeviceAuthModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }
    }
}