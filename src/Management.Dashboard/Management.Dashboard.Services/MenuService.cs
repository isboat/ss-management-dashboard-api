using Management.Dashboard.Models;
using Management.Dashboard.Repositories.Interfaces;
using Management.Dashboard.Services.Interfaces;

namespace Management.Dashboard.Services
{
    public class MenuService : IMenuService
    {
        private readonly IRepository<MenuModel> _repository;

        public MenuService(IRepository<MenuModel> repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<MenuModel>> GetMenusAsync(string tenantId) =>
            await _repository.GetAllByTenantIdAsync(tenantId);

        public async Task<MenuModel?> GetAsync(string tenantId, string id) =>
            await _repository.GetAsync(tenantId, id);

        public async Task CreateAsync(MenuModel newModel)
        {
            AddId(newModel);
            foreach (var item in newModel.MenuItems ?? new List<MenuItem>())
            {
                AddId(item);
            }
            await _repository.CreateAsync(newModel);
        }

        public async Task RemoveAsync(string tenantId, string id) =>
            await _repository.RemoveAsync(tenantId, id);

        public async Task UpdateAsync(string id, MenuModel updatedModel)
        {
            await _repository.UpdateAsync(id, updatedModel);
        }

        private static void AddId(IModelItem newModel)
        {
            newModel.Id = Guid.NewGuid().ToString("N");
        }
    }
}