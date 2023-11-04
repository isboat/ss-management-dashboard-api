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

            var mongoDatabase = _client.GetDatabase("DeviceCodeRegistration");

            _collection = mongoDatabase.GetCollection<DeviceAuthModel>("Users");
        }

        public async Task<List<DeviceAuthModel>> GetAllByTenantIdAsync(string tenantId) =>
            await _collection.Find(x => x.TenantId == tenantId).ToListAsync();

        public async Task<List<DeviceAuthModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

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

            existingUser.DeviceName = updateModel?.DeviceName;

            await _collection.ReplaceOneAsync(x => x.Id == id, existingUser);
        }

        public async Task ApproveAsync(DeviceAuthModel updateModel)
        {
            if (updateModel == null) return;

            var existingUser = await _collection.Find(x => x.UserCode == updateModel.UserCode).FirstOrDefaultAsync();
            if (existingUser == null) return;

            existingUser.ApprovedDatetime = DateTime.UtcNow;
            existingUser.TenantId = updateModel?.TenantId;

            await _collection.ReplaceOneAsync(x => x.Id == existingUser.Id, existingUser);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id && x.TenantId == tenantId);
    }
}