using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Management.Dashboard.Repositories
{
    public class PublishRepository : IPublishRepository
    {
        private readonly MongoClient _client;
        private readonly string PublishedScreenColName = "PublishedScreenData";
        private readonly string ArchivedScreenColName = "ArchivedScreenData";

        public PublishRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(
            settings.Value.ConnectionString);
            var objectSerializer = new ObjectSerializer(
                type => ObjectSerializer.DefaultAllowedTypes(type) 
                || type.FullName.StartsWith("Management.Dashboard")); 
            
            BsonSerializer.RegisterSerializer(objectSerializer);
        }

        public async Task<bool> PublishScreenAsync(DetailedScreenModel model)
        {
            var collection = GetCollection<DetailedScreenModel>(model.TenantId!, this.PublishedScreenColName);
            var result = await collection.ReplaceOneAsync(x => x.Id == model.Id, model, new ReplaceOptions { IsUpsert = true});
            return result.IsAcknowledged;
        }

        public async Task<bool> ArchiveScreenAsync(string tenantId, string id)
        {
            var publishCol = GetCollection<DetailedScreenModel>(tenantId, this.PublishedScreenColName);
            var publishedScreen = await publishCol.Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();

            if (publishedScreen == null) return false;

            var archiveCol = GetCollection<DetailedScreenModel>(tenantId, this.ArchivedScreenColName);
            var result = await archiveCol.ReplaceOneAsync(x => x.Id == publishedScreen.Id, publishedScreen, new ReplaceOptions { IsUpsert = true });
            
            if(!result.IsAcknowledged) return false;

            var deletionRes = await publishCol.DeleteOneAsync(x => x.Id == publishedScreen.Id);
            return deletionRes.IsAcknowledged;
        }

        private IMongoCollection<T> GetCollection<T>(string tenantId, string collectionName)
        {
            var mongoDatabase = _client.GetDatabase(tenantId);
            return mongoDatabase.GetCollection<T>(collectionName);
        }
    }
}