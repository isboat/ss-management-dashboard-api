using Management.Dashboard.Models;
using Management.Dashboard.Models.History;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IRepository<ScreenModel> _repository;
        private readonly IHistoryService _historyService;

        public ScreenService(IRepository<ScreenModel> repository, IHistoryService historyService)
        {
            _repository = repository;
            _historyService = historyService;
        }

        public async Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<ScreenModel?> GetAsync(string tenantId, string id)
        {
            var screen = await _repository.GetAsync(tenantId, id);
            if (screen == null) return null;

            screen.Layout ??= new();
            screen.Layout.TemplateProperties ??= new List<TemplatePropertyModel>();

            return screen;
        }

        public async Task CreateAsync(ScreenModel newModel, string creator)
        {
            AddId(newModel);
            await _repository.CreateAsync(newModel); 

            var itemType = nameof(ScreenModel);

            await StoreHistory(newModel.TenantId!, creator, newModel.Id!, $"Created New {itemType} - {newModel.DisplayName}");
        }

        public async Task RemoveAsync(string tenantId, string id, string deletor)
        {
            await _repository.RemoveAsync(tenantId, id);

            await StoreHistory(tenantId, deletor, id, $"Removed");
        }

        public async Task UpdateAsync(string id, ScreenModel updatedModel, string updator)
        {
            await _repository.UpdateAsync(id, updatedModel);
            await StoreHistory(updatedModel.TenantId!, updator, id, $"Updated - {updatedModel.DisplayName}");
        }

        private async Task StoreHistory(string tenantId, string user, string itemId, string logMsg)
        {
            var itemType = nameof(ScreenModel);

            await _historyService.StoreAsync(new HistoryModel
            {
                ItemId = itemId,
                ItemType = itemType,
                Log = logMsg,
                TenantId = tenantId,
                User = user,
            });
        }

        private static void AddId(ScreenModel newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}