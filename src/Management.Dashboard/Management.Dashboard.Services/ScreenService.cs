using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class ScreenService : IScreenService
    {
        private readonly IScreenRepository _screenRepository;

        public ScreenService(IScreenRepository screenRepository)
        {
            _screenRepository = screenRepository;
        }

        public async Task<IEnumerable<ScreenModel>> GetScreensAsync(string tenantId) =>
            await _screenRepository.GetScreensAsync(tenantId);

        public async Task<ScreenModel?> GetAsync(string tenantId, string id) =>
            await _screenRepository.GetAsync(tenantId, id);

        public async Task CreateAsync(ScreenModel newModel)
        {
            AddId(newModel);
            await _screenRepository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _screenRepository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, ScreenModel updatedModel) =>
            await _screenRepository.UpdateAsync(id, updatedModel);

        private static void AddId(ScreenModel newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}