using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetItemModel>> GetAllAsync(string tenantId, int? skip, int? limit);

        Task<AssetItemModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(AssetItemModel newModel);

        public Task UpdateAsync(string id, AssetItemModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
