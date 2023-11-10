using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class TenantRepository : ITenantRepository
    {
        private readonly IMongoCollection<TenantModel> _collection;
        private readonly MongoClient _client;
        private readonly string dbName = "TenantAdmin";
        public TenantRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);

            var mongoDatabase = _client.GetDatabase(dbName);

            _collection = mongoDatabase.GetCollection<TenantModel>("Tenants");
        }

        public async Task<IEnumerable<TenantModel>> GetAllAsync() =>
            await _collection.Find(_ => true).ToListAsync();

        public async Task<TenantModel?> GetAsync(string id) =>
            await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
    }
}