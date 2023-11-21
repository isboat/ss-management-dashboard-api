using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class PublishRepository : IPublishRepository
    {
        private readonly MongoClient _client;
        private readonly string PublishedScreenColName = "PublishedScreenData";

        public PublishRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
        }

        public async Task<bool> PublishScreenAsync(DetailedScreenModel model)
        {
            var collection = GetCollection<DetailedScreenModel>(model.TenantId!, this.PublishedScreenColName);
            var result = await collection.ReplaceOneAsync(x => x.Id == model.Id, model, new ReplaceOptions { IsUpsert = true});
            return result.IsAcknowledged;
        }

        private IMongoCollection<T> GetCollection<T>(string tenantId, string collectionName)
        {
            var mongoDatabase = _client.GetDatabase(tenantId);
            return mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}