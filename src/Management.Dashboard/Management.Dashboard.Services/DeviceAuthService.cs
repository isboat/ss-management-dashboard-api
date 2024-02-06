using Management.Dashboard.Common;
using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class DeviceAuthService : IDeviceAuthService
    {
        private readonly IDeviceAuthRepository<DeviceAuthModel> _repository;
        private readonly ITenantRepository _tenantRepository;
        private readonly IDateTimeProvider _dateTimeProvider;

        public DeviceAuthService(
            IDeviceAuthRepository<DeviceAuthModel> repository, 
            IDateTimeProvider dateTimeProvider, 
            ITenantRepository tenantRepository)
        {
            _repository = repository;
            _dateTimeProvider = dateTimeProvider;
            _tenantRepository = tenantRepository;
        }
        public async Task<DeviceAuthApprovalStatus> ApproveAsync(DeviceAuthModel updatedModel)
        {
            var tenant = await _tenantRepository.GetAsync(updatedModel.TenantId!);
            if (string.IsNullOrEmpty(tenant?.Id)) return DeviceAuthApprovalStatus.TenantNotFound;

            var currentTvs = await _repository.GetAllByTenantIdAsync(tenant.Id, null, null);
            if (currentTvs.Count >= tenant.TvAppsLimit)
            {
                return DeviceAuthApprovalStatus.DeviceLimitReached;
            }

            return await _repository.ApproveAsync(updatedModel);
        }

        public async Task CreateAsync(DeviceAuthModel newModel)
        {
            await _repository.CreateAsync(newModel);
        }

        public async Task<DeviceAuthModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<IEnumerable<DeviceAuthModel>> GetApprovedDevicesAsync(string tenantId)
        {
            var items = await _repository.GetAllByTenantIdAsync(tenantId, null, null);
            return items.Where(x => IsApproved(x));
        }

        public async Task UpdateAsync(string id, DeviceAuthModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        public async Task DeleteAsync(string tenantId, string id)
        {
            await _repository.RemoveAsync(tenantId, id);
        }

        private bool IsApproved(DeviceAuthModel model)
        {
            return model?.ApprovedDatetime != null
                && model.ApprovedDatetime > _dateTimeProvider.UnixEpoch!.Value
                && model.ApprovedDatetime > model.RegisteredDatetime;
        }
    }
}