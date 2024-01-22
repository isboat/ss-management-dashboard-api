using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Extensions;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class TextAssetService : ITextAssetService
    {
        private readonly IRepository<TextAssetItemModel> _repository;

        public TextAssetService(IRepository<TextAssetItemModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<TextAssetItemModel>> GetAllAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<TextAssetItemModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task<string> CreateAsync(TextAssetItemModel newModel)
        {
            newModel.AddId();
            await _repository.CreateAsync(newModel);
            return newModel.Id!;
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, TextAssetItemModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }
    }
}