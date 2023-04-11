using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface IAssetService
    {
        Task<IEnumerable<AssetItemModel>> GetMenusAsync(string tenantId);

        Task<AssetItemModel?> GetAsync(string tenantId, string id);

        public Task CreateAsync(AssetItemModel newModel);

        public Task UpdateAsync(string id, AssetItemModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
