using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class DeviceAuthRepository : IDeviceAuthRepository<DeviceAuthModel>
    {
        private readonly IMongoCollection<DeviceAuthModel> _collection;
        private readonly MongoClient _client;

        public DeviceAuthRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("device-app-db");

            _collection = mongoDatabase.GetCollection<DeviceAuthModel>("DeviceCodeRegistration");
        }

        public async Task<List<DeviceAuthModel>> GetAllByTenantIdAsync(string tenantId)
        {
            var items = _collection.Find(x => x.TenantId == tenantId);
            return items != null ? await items.ToListAsync() : new List<DeviceAuthModel>();
        }

        public async Task<List<DeviceAuthModel>> GetAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<DeviceAuthModel?> GetAsync(string tenantId, string id) =>
            await _collection.Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();

        public async Task<DeviceAuthModel?> GetByDeviceUsercodeAsync(string deviceUsercode) =>
            await _collection.Find(x => x.UserCode == deviceUsercode).FirstOrDefaultAsync();

        public async Task CreateAsync(DeviceAuthModel newTenant) =>
            await _collection.InsertOneAsync(newTenant);

        public async Task UpdateAsync(string id, DeviceAuthModel updateModel)
        {
            if (updateModel == null) return;

            var existingUser = await this.GetAsync(updateModel?.TenantId!, id);
            if (existingUser == null) return;

            if (!string.IsNullOrEmpty(updateModel?.DeviceName))
                existingUser.DeviceName = updateModel?.DeviceName;

            if (!string.IsNullOrEmpty(updateModel?.ScreenId))
                existingUser.ScreenId = updateModel?.ScreenId;

            await _collection.ReplaceOneAsync(x => x.Id == id, existingUser);
        }

        public async Task<DeviceAuthApprovalStatus> ApproveAsync(DeviceAuthModel updateModel)
        {
            if (updateModel == null) return DeviceAuthApprovalStatus.BadRequest;

            var existingUser = await _collection.Find(x => x.UserCode == updateModel.UserCode).FirstOrDefaultAsync();
            if (existingUser == null) return DeviceAuthApprovalStatus.NotFound;
            if (existingUser.ApprovedDatetime != null) return DeviceAuthApprovalStatus.AlreadyApproved;

            existingUser.ApprovedDatetime = DateTime.UtcNow;
            existingUser.TenantId = updateModel?.TenantId;

            var result = await _collection.ReplaceOneAsync(x => x.Id == existingUser.Id, existingUser);
            return result.IsAcknowledged ? DeviceAuthApprovalStatus.Success : DeviceAuthApprovalStatus.Failed;
        }

        public async Task RemoveAsync(string tenantId, string id)
        {
            var result = await _collection.DeleteOneAsync(x => x.Id == id && x.TenantId == tenantId);
        }
    }
}