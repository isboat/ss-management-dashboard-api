using Management.Dashboard.Models;

namespace Management.Dashboard.Services.Interfaces
{
    public interface ITextAssetService
    {
        Task<IEnumerable<TextAssetItemModel>> GetAllAsync(string tenantId);

        Task<TextAssetItemModel?> GetAsync(string tenantId, string id);

        public Task<string> CreateAsync(TextAssetItemModel newModel);

        public Task UpdateAsync(string id, TextAssetItemModel updatedModel);

        public Task RemoveAsync(string tenantId, string id);
    }
}
