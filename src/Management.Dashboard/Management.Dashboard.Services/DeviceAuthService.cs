using Management.Dashboard.Common;
using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class DeviceAuthService : IDeviceAuthService
    {
        private readonly IDeviceAuthRepository<DeviceAuthModel> _repository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceAuthService(IDeviceAuthRepository<DeviceAuthModel> repository, IDateTimeProvider dateTimeProvider)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
        }
        public async Task<DeviceAuthApprovalStatus> ApproveAsync(DeviceAuthModel updatedModel) =>
            await _repository.ApproveAsync(updatedModel);

        public async Task CreateAsync(DeviceAuthModel newModel)
        {
            await _repository.CreateAsync(newModel);
        }

        public async Task<DeviceAuthModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<IEnumerable<DeviceAuthModel>> GetApprovedDevicesAsync(string tenantId)
        {
            var items = await _repository.GetAllByTenantIdAsync(tenantId);
            return items.Where(x => IsApproved(x));
        }

        public async Task UpdateAsync(string id, DeviceAuthModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        private bool IsApproved(DeviceAuthModel model)
        {
            return model?.ApprovedDatetime != null
                && model.ApprovedDatetime > _dateTimeProvider.UnixEpoch!.Value
                && model.ApprovedDatetime > model.RegisteredDatetime;
        }
    }
}