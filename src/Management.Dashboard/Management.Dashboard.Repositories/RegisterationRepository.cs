using Management.Dashboard.Models;
using Management.Dashboard.Models.Authentication;
using Management.Dashboard.Models.Settings;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections;

namespace Management.Dashboard.Repositories
{
    public class RegisterationRepository : IRegisterationRepository
    {
        private readonly IMongoCollection<RegisterModel> _collection;
        private readonly MongoClient _client;

        public RegisterationRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase("TenantAdmin");

            _collection = mongoDatabase.GetCollection<RegisterModel>("Registerations");
        }

        public async Task<List<RegisterModel>> GetAllByTenantIdAsync(string tenantId, int? skip, int? limit)
        {
            return await _collection.Find(x => x.Id == tenantId).Skip(skip).Limit(limit).ToListAsync();
        }

        public async Task<List<RegisterModel>> GetAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<RegisterModel?> GetAsync(string tenantId, string id) =>
            await _collection.Find(x => x.Id == id && x.Id == tenantId).FirstOrDefaultAsync();


        public async Task<RegisterModel?> GetByEmailAsync(string email) =>
            await _collection.Find(x => x.Email == email).FirstOrDefaultAsync();

        public async Task CreateAsync(RegisterModel newModel)
        {
            newModel.Created = DateTime.UtcNow;
            await _collection.InsertOneAsync(newModel);
        }

        public Task UpdateAsync(string id, RegisterModel updateModel)
        {
            return Task.CompletedTask;
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _collection.DeleteOneAsync(x => x.Id == id);

        public async Task<IEnumerable<RegisterModel>> GetByFilterAsync(string tenantId, FilterDefinition<RegisterModel> filter)
        {
            return await _collection.Find(filter).ToListAsync();
        }
    }
}