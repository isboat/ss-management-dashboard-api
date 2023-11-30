﻿using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IDeviceAuthService
    {
        public Task<DeviceAuthApprovalStatus> ApproveAsync( DeviceAuthModel updatedModel);

        Task<IEnumerable<DeviceAuthModel>> GetApprovedDevicesAsync(string tenantId);

        Task<DeviceAuthModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(DeviceAuthModel newModel);

        public Task UpdateAsync(string id, DeviceAuthModel updatedModel);

        public Task DeleteAsync(string tenantId, string id);
    }
}