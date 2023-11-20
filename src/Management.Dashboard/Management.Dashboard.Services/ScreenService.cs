using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IRepository<ScreenModel> _repository;

        public ScreenService(IRepository<ScreenModel> repository)
        {
            _repository = repository;
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

        public async Task CreateAsync(ScreenModel newModel)
        {
            AddId(newModel);
            await _repository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, ScreenModel updatedModel) =>
            await _repository.UpdateAsync(id, updatedModel);

        private static void AddId(ScreenModel newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}