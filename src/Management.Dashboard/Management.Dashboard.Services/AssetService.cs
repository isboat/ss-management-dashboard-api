﻿using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class AssetService : IAssetService
    {
        private readonly IRepository<AssetItemModel> _repository;

        public AssetService(IRepository<AssetItemModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AssetItemModel>> GetMenusAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<AssetItemModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task CreateAsync(AssetItemModel newModel)
        {
            AddId(newModel);
            await _repository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, AssetItemModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}