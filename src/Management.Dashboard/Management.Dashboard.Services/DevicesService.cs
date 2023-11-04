using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class DevicesService : IDevicesService
    {
        private readonly IRepository<DeviceModel> _repository;

        public DevicesService(IRepository<DeviceModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<DeviceModel>> GetDevicesAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<DeviceModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task CreateAsync(DeviceModel newModel)
        {
            AddId(newModel);
            await _repository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, DeviceModel updatedModel) =>
            await _repository.UpdateAsync(id, updatedModel);

        private static void AddId(DeviceModel newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}