﻿using Management.Dashboard.Models;
using Management.Dashboard.Models.Settings;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections;

namespace Management.Dashboard.Repositories
{
    public class BaseRepository
    {
        protected readonly MongoClient _client;

        public BaseRepository(IOptions<MongoSettings> settings)
        {
            _client = new MongoClient(settings.Value.ConnectionString);
        }

        protected IMongoCollection<T> GetTenantCollection<T>(string tenantId, string collectionName)
        {
            var database = _client.GetDatabase(tenantId);
            return database.GetCollection<T>(collectionName);
        }

        protected async Task<List<T>> GetAllByTenantIdAsync<T>(string tenantId, string collectionName, int? skip, int? limit) where T: IModelItem
        {
            return await GetTenantCollection<T>(tenantId, collectionName)
                .Find(x => x.TenantId == tenantId).Skip(skip).Limit(limit).ToListAsync();
        }

        protected async Task<List<T>> GetAsync<T>(string tenantId, string collectionName)
        {
            return await GetTenantCollection<T>(tenantId, collectionName).Find(_ => true).ToListAsync();
        }

        protected async Task<T?> GetAsync<T>(string tenantId, string collectionName, string id) where T : IModelItem =>
            await GetTenantCollection<T>(tenantId, collectionName).Find(x => x.Id == id && x.TenantId == tenantId).FirstOrDefaultAsync();


        protected async Task CreateAsync<T>(string tenantId, string collectionName, T newTenant) where T : IModelItem =>
            await GetTenantCollection<T>(tenantId, collectionName).InsertOneAsync(newTenant);

        protected async Task RemoveAsync<T>(string tenantId, string collectionName, string id) where T : IModelItem
        {
            var result = await GetTenantCollection<T>(tenantId, collectionName).DeleteOneAsync(x => x.Id == id && x.TenantId == tenantId);
        }
    }
}